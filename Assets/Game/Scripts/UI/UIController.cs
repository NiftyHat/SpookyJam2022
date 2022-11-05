using System;
using Context;
using Entity;
using Interactions;
using Interactions.Commands;
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
        [SerializeField] [NonNull] private RadiusIndicatorView _radiusIndicatorView;
        
        private FloorLocation _floorLocation;
        
        //TODO - make this generic.
        private PlayerInputHandler _player;
        private GameStateContext _gameStateContext;
        
        protected InteractionCommand _previewCommand;
        public InteractionCommand PreviewCommand => _previewCommand;


        public void Start()
        {
            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
            }
            _unitInputController.OnSelectUnit += HandleInputUnitSelect;
            _unitInputController.OnSelectGround += HandleInputSelectGround;
            _unitInputController.OnOverGround += HandleInputOverGround;
            _selectedTargetView.OnPreviewCommand += HandlePreviewCommand;
            ContextService.Get<GameStateContext>(HandleGameState);
        }

        private void HandlePreviewCommand(InteractionCommand command)
        {
            _previewCommand = command;
        }

        private void HandleInputOverGround(MovementPlaneView movementPlane, RaycastHit raycastHit)
        {
            if (_floorLocation == null)
            {
                _floorLocation = new FloorLocation(raycastHit.point);
            }
            
            if (_previewCommand != null)
            {
                Vector3 sourcePosition = _previewCommand.Targets.Source.GetInteractionPosition();
                float rangeMax = _previewCommand.Range.Max;
                float rangeMin = _previewCommand.Range.Min;
                Vector3 direction = _previewCommand.Targets.DirectionToTarget();
                float distanceToTarget = _previewCommand.Targets.GetDistance();
                if (distanceToTarget > rangeMax)
                {
                    Vector3 maxPossibleDistance = direction * rangeMax;
                    _floorLocation.Set(sourcePosition + maxPossibleDistance);
                }
                else if (distanceToTarget < rangeMin)
                {
                    Vector3 minPossibleDistance = direction * rangeMax;
                    _floorLocation.Set(sourcePosition + minPossibleDistance);
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
            
            if (_previewCommand != null)
            {
                if (_previewCommand.IsValidTarget(_floorLocation))
                {
                    _previewCommand.SetTarget(_floorLocation);
                }
            }
        }

        private void HandleGameState(GameStateContext service)
        {
            _gameStateContext = service;
            service.GetPlayer(player => _player = player);
        }


        private void HandleInputSelectGround(MovementPlaneView groundPlane, RaycastHit raycastHit)
        {
            if (_previewCommand != null)
            {
                if (_previewCommand.IsValidTarget(_floorLocation))
                {
                    _previewCommand.SetTarget(_floorLocation);
                    _gameStateContext.RunCommand(_previewCommand);
                    ClearPreview();
                }
            }
        }
        
        private void HandleInputUnitSelect(UnitInputHandler unit)
        {
            SetSelectedUnit(unit);
        }
        
        public void SetPreview(InteractionCommand command)
        {
            _previewCommand = command;
        }

        public void RemovePreview(InteractionCommand command)
        {
            if (_previewCommand == command)
            {
                _previewCommand = null;
            }
        }

        public void SetSelectedUnit(UnitInputHandler selectedInputHandler)
        {
            _selectedUnit = selectedInputHandler;
            if (_selectedUnit is PlayerInputHandler playerInputHandler)
            {
                _selectedTargetView.Set(playerInputHandler);
                if (_previewCommand == null)
                {
                    SetPreview(playerInputHandler.GetDefaultCommand());
                }
            }
            else if (_selectedUnit is ITargetable<CharacterEntity> targetCharacter)
            {
                _selectedTargetView.Set(targetCharacter);
                if (_previewCommand != null)
                {
                    _previewCommand.SetTarget(targetCharacter);
                }
            }
            else
            {
                ClearPreview();
            }
        }

        private void ClearPreview()
        {
            _rangeIndicatorView.Clear();
            _locationIndicatorView.Clear();
            _selectedTargetView.Clear();
            _radiusIndicatorView.Clear();
            _previewCommand = null;
        }

        protected void Update()
        {
            if (_previewCommand != null)
            {
                if (_player != null)
                {
                    _player.PreviewAPCost(_previewCommand);
                }
                _previewCommand.Validate();
                TargetingInfo targetInfo = _previewCommand.Targets;
                if (targetInfo.Target != null)
                {
                    Vector3 sourcePos = targetInfo.Source.GetInteractionPosition();
                    Vector3 targetPos = targetInfo.Target.GetInteractionPosition();
                    if (_previewCommand.ShowRangeCircle)
                    {
                        if (!_rangeIndicatorView.gameObject.activeSelf)
                        {
                            _rangeIndicatorView.gameObject.SetActive(true);
                        }
                        float maxRange = _previewCommand.Interaction.RangeMax;
                        _rangeIndicatorView.ShowDistance(sourcePos, maxRange, _previewCommand.ValidateRange);
                    }
                    else
                    {
                        if (_rangeIndicatorView.gameObject.activeSelf)
                        {
                            _rangeIndicatorView.Clear();
                        }
                    }

                    if (_previewCommand.ShowTargetLine)
                    {
                        if (!_locationIndicatorView.gameObject.activeSelf)
                        {
                            _locationIndicatorView.gameObject.SetActive(true);
                        }

                        _locationIndicatorView.Set(sourcePos,
                            targetPos, _previewCommand.ValidateRange);
                    }
                    else
                    {
                        if (_locationIndicatorView.gameObject.activeSelf)
                        {
                            _locationIndicatorView.Clear();
                        }

                    }

                    if (_previewCommand.ShowRadiusCircle)
                    {
                        if (!_radiusIndicatorView.gameObject.activeSelf)
                        {
                            _radiusIndicatorView.gameObject.SetActive(true);
                        }

                        float radius = _previewCommand.Interaction.Radius;
                        _radiusIndicatorView.ShowRadius(sourcePos, radius, _previewCommand.ValidateRadiusTargets);
                    }
                    else
                    {
                        if (_radiusIndicatorView.gameObject.activeSelf)
                        {
                            _radiusIndicatorView.Clear();
                        }
                    }
                }
            }
        }
    }
}