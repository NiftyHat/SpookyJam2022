using System;
using Context;
using Data.Interactions;
using Entity;
using Interactions;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using TouchInput.UnitControl;
using UI.Targeting;
using UnityEngine;


namespace UI
{
    public class UIController : MonoBehaviour
    {
        public class FloorLocation : ITargetable
        {
            private Vector3 _position;

            public FloorLocation(Vector3 position)
            {
                _position = position;
            }

            public void Set(Vector3 position)
            {
                _position = position;
                OnPositionUpdate?.Invoke(_position);
            }
            
            public Vector3 GetInteractionPosition()
            {
                return _position;
            }

            public GameObject GetGameObject()
            {
                return null;
            }

            public bool TryGetGameObject(out GameObject gameObject)
            {
                gameObject = null;
                return false;
            }

            public event Action<Vector3> OnPositionUpdate;
        }
        
        private UnitInputHandler _selectedUnit;
        [SerializeField] private LocationIndicatorView _locationIndicatorView;
        [SerializeField] private UnitInputController _unitInputController;
        [SerializeField] [NonNull] private RangeIndicatorView _rangeIndicatorView;
        [SerializeField] [NonNull] private UISelectedTargetView _selectedTargetView;
        
        private InteractionState _previewInteraction;
        private FloorLocation _floorLocation;
        
        //TODO - make this generic.
        private PlayerInputHandler _player;

        public void Start()
        {
            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
            }
            _unitInputController.OnSelectUnit += HandleInputUnitSelect;
            _unitInputController.OnSelectGround += HandleInputSelectGround;
            _unitInputController.OnOverGround += HandleInputOverGround;
            _selectedTargetView.OnPreviewInteraction += HandlePreviewInteraction;
            ContextService.Get<GameStateContext>(HandleGameState);
        }

        private void HandlePreviewInteraction(InteractionState interactionState)
        {
            SetPreview(interactionState);
        }

        private void HandleInputOverGround(MovementPlaneView movementPlane, RaycastHit raycastHit)
        {
            if (_floorLocation == null)
            {
                _floorLocation = new FloorLocation(raycastHit.point);
            }
            if (_previewInteraction != null)
            {

                float rangeMax = _previewInteraction.GetMaxRange();
                float rangeMin = _previewInteraction.GetMinRange();
                Vector3 direction = _previewInteraction.TargetInfo.DirectionToTarget();
                float distanceToTarget = _previewInteraction.TargetInfo.GetDistance();
                if (distanceToTarget > rangeMax)
                {
                    Vector3 maxPossibleDistance = direction * rangeMax;
                    _floorLocation.Set(_previewInteraction.TargetInfo.Source.GetInteractionPosition() + maxPossibleDistance);
                }
                else if (distanceToTarget < rangeMin)
                {
                    Vector3 minPossibleDistance = direction * rangeMax;
                    _floorLocation.Set(_previewInteraction.TargetInfo.Source.GetInteractionPosition() + minPossibleDistance);
                }
                else
                {
                    _floorLocation.Set(raycastHit.point);
                }
                _floorLocation.Set(raycastHit.point);
            }
            else
            {
                _floorLocation.Set(raycastHit.point);
            }
            
            if (_previewInteraction != null)
            {
                if (_previewInteraction.IsValidTarget(_floorLocation))
                {
                    _previewInteraction.SetTarget(_floorLocation);
                }
            }
        }

        private void HandleGameState(GameStateContext service)
        {
            service.GetPlayer((player) =>
            {
                _player = player;
                if (_previewInteraction == null)
                {
                    var defaultInteraction = _player.GetDefaultInteraction();
                    SetInteraction(defaultInteraction, player);
                }
            });
        }

        public void SetInteraction(InteractionData interactionData, ITargetable source)
        {
            if (_selectedUnit != null && interactionData.IsValidTarget(_selectedUnit))
            {
                _previewInteraction = new InteractionState(interactionData, source, _selectedUnit);
                return;
            }
            if (_floorLocation != null && interactionData.IsValidTarget(_floorLocation))
            {
                _previewInteraction = new InteractionState(interactionData, source, _floorLocation);
                return;
            }
            _previewInteraction = new InteractionState(interactionData, source);
        }

        public void SetPreview(InteractionState interactionState)
        {
            if (interactionState != null)
            {
                Debug.Log($"SetPreview({interactionState.GetInteractionName()})");
                _previewInteraction = interactionState;
            }
            else
            {
                Debug.Log($"SetPreview(null)");
                PreviewDefaultAction();
            }
        }


        private void HandleInputSelectGround(MovementPlaneView groundPlane, RaycastHit raycastHit)
        {
            _floorLocation.Set(raycastHit.point);
            if (_previewInteraction != null && _previewInteraction.IsValidTarget(_floorLocation))
            {
                _previewInteraction.Run(HandleActionRun, HandleActionComplete, HandleActionFail);
            }
        }

        private void HandleActionComplete()
        {
            if (_previewInteraction == null)
            {
                PreviewDefaultAction();
            }
        }

        private void PreviewDefaultAction()
        {
            if (_player != null)
            {
                var defaultInteraction = _player.GetDefaultInteraction();
                if (defaultInteraction != null)
                {
                    SetInteraction(defaultInteraction, _player);
                }
            }
        }

        private void HandleActionFail(InteractionState arg1, string arg2)
        {
            Debug.LogError(arg2);
        }

        private void HandleActionRun(InteractionState interactionState)
        {
            _previewInteraction = null;
            if (_rangeIndicatorView != null)
            {
                _rangeIndicatorView.Clear();
            }
            if (_locationIndicatorView != null)
            {
                _locationIndicatorView.Clear();
            }
        }

        private void HandleInputUnitSelect(UnitInputHandler unit)
        {
            SetSelectedUnit(unit);
        }

        public void SetSelectedUnit(UnitInputHandler selectedInputHandler)
        {
            _selectedUnit = selectedInputHandler;
            if (_selectedUnit == _player)
            {
                SetInteraction(_player.GetDefaultInteraction(), _player);
            }
            else if (_selectedUnit is ITargetable<CharacterEntity> targetCharacter)
            {
                _selectedTargetView.Set(targetCharacter);
                if (_previewInteraction != null)
                {
                    _previewInteraction.SetTarget(targetCharacter);
                }
            }
            else
            {
                _rangeIndicatorView.Clear();
                _locationIndicatorView.Clear();
                _selectedTargetView.Clear();
                _previewInteraction = null;
            }
        }

        protected void Update()
        {
            if (_previewInteraction != null)
            {
                _previewInteraction.Update();
                TargetingInfo targetInfo = _previewInteraction.TargetInfo;
                if (targetInfo.Target != null)
                {
                    Vector3 sourcePos = targetInfo.Source.GetInteractionPosition();
                    Vector3 targetPos = targetInfo.Target.GetInteractionPosition();
                    if (_previewInteraction.ShowRangeCircle)
                    {
                        if (!_rangeIndicatorView.gameObject.activeSelf)
                        {
                            _rangeIndicatorView.gameObject.SetActive(true);
                        }
                        float maxRange = _previewInteraction.GetMaxRange();
                        _rangeIndicatorView.ShowDistance(sourcePos, maxRange, _previewInteraction.ValidateRange);
                    }

                    if (_previewInteraction.ShowTargetLine)
                    {
                        if (!_locationIndicatorView.gameObject.activeSelf)
                        {
                            _locationIndicatorView.gameObject.SetActive(true);
                        }

                        _locationIndicatorView.Set(sourcePos,
                            targetPos, _previewInteraction.ValidateRange);
                    }
                }
            }
        }
    }
}