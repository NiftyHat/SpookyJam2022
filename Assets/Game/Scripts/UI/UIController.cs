using System;
using Context;
using Data.Interactions;
using Data.Reactions;
using Entity;
using Interactions;
using Interactions.Commands;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using TouchInput.UnitControl;
using UI.Audio;
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
        
        private PointerSelectionHandler _selected;
        private PointerSelectionHandler _selectedOver;
        [SerializeField] private LocationIndicatorView _locationIndicatorView;
        [SerializeField] private PointerSelectInputController _pointerSelectInputController;
        [SerializeField] [NonNull] private RangeIndicatorView _rangeIndicatorView;
        [SerializeField] [NonNull] private RadiusIndicatorView _radiusIndicatorView;
        [SerializeField] [NonNull] private UIInteractionListPanel _interactionList;
        [SerializeField] [NonNull] private UICharacterSelectPreview _characterSelectPreview;
        [SerializeField] [NonNull] private UIPlayerInfo _playerInfoView;
        [SerializeField] [NonNull] private UIAudioSelection _uiAudioSelection;
        
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
            _pointerSelectInputController.OnSelectionChanged += HandleInputSelection;
            _pointerSelectInputController.OnSelectionOverChanged += HandleOverSelection;
            _pointerSelectInputController.OnSelectGround += HandleInputSelectGround;
            _pointerSelectInputController.OnOverGround += HandleInputOverGround;
            _interactionList.OnPreviewCommand += HandlePreviewCommand;
            _playerInfoView.OnSelectPlayer += HandleSelectPlayer;
            ContextService.Get<GameStateContext>(HandleGameState);
        }

        private void HandleSelectPlayer()
        {
            if (_player != null && _player.TryGetComponent<PointerSelectionHandler>(out var handler))
            {
                _pointerSelectInputController.SetSelection(handler);
            }
        }

        private void HandleOverSelection(PointerSelectionHandler obj, RaycastHit raycastHit)
        {
            if (obj == null || obj.Target == null)
            {
                return;
            }
            if (obj.Target is TransitionZoneView transitionZoneView)
            {
                if (_previewCommand != null && _previewCommand.IsValidTarget(_floorLocation) && _previewCommand.Targets.Target != obj.Target)
                {
                    //_floorLocation.Set(raycastHit.point);
                    _previewCommand.SetTarget(transitionZoneView);
                }
            }
        }

        private void HandlePreviewCommand(InteractionCommand command)
        {
            _previewCommand = command;
        }

        private void HandleInputOverGround(WalkLocationView walkLocation, RaycastHit raycastHit)
        {
            if (_floorLocation == null)
            {
                _floorLocation = new FloorLocation(raycastHit.point);
            }
            else
            {
                _floorLocation.Set(raycastHit.point);
            }
            
            if (_previewCommand != null)
            {
                if (_previewCommand.IsValidTarget(_floorLocation))
                {
                    if (_previewCommand.Targets.Target != _floorLocation)
                    {
                        _floorLocation.Set(raycastHit.point);
                        _previewCommand.SetTarget(_floorLocation);  
                    }
                    _pointerSelectInputController.SetCanSelect(false);
                }
                else
                {
                    _pointerSelectInputController.SetCanSelect(true);
                }
                
                Vector3 sourcePosition = _previewCommand.Targets.Source.GetInteractionPosition();
                float rangeMax = _previewCommand.Range.Max;
                float rangeMin = _previewCommand.Range.Min;
                Vector3 direction = _previewCommand.Targets.DirectionTo(raycastHit.point);
                float distanceToTarget = _previewCommand.Targets.GetDistance(raycastHit.point);
               
                if (distanceToTarget > rangeMax)
                {
                    Vector3 maxPossibleDistance = direction * rangeMax;
                    _floorLocation.Set(sourcePosition + maxPossibleDistance);
                }
                else if (distanceToTarget < rangeMin)
                {
                    Vector3 minPossibleDistance = direction * rangeMin;
                    _floorLocation.Set(sourcePosition + minPossibleDistance);
                }
                else
                {
                    _floorLocation.Set(raycastHit.point);
                }
            }
        }

        private void HandleGameState(GameStateContext service)
        {
            _gameStateContext = service;
            _gameStateContext.GetPlayer(player => _player = player);
            _gameStateContext.OnReset += HandleGameStateReset;
        }

        private void HandleGameStateReset()
        {
            _gameStateContext.OnReset -= HandleGameStateReset;
            _gameStateContext = null;
            _player = null;
        }

        private void HandleInputSelectGround(WalkLocationView groundPlane, RaycastHit raycastHit)
        {
            if (_previewCommand != null)
            {
                if (_previewCommand.IsValidTarget(_floorLocation))
                {
                    if (_uiAudioSelection != null)
                    {
                        _uiAudioSelection.PlayGroundSelected();
                    }
                    _previewCommand.SetTarget(_floorLocation);
                    _gameStateContext.RunCommand(_previewCommand);
                    ClearPreview();
                }
            }
        }
        
        private void HandleInputSelection(PointerSelectionHandler unit)
        {
            SetSelected(unit);
        }
        
        public void SetPreview(InteractionCommand command)
        {
            _previewCommand = command;
            if (_selected.Target != null)
            {
                _previewCommand.SetTarget(_selected.Target);
            }
        }

        public void SetSelected(PointerSelectionHandler selected)
        {
            if (_uiAudioSelection != null)
            {
                if (selected != null)
                {
                    _uiAudioSelection.PlaySelected();
                }
                else
                {
                    _uiAudioSelection.PlaySelectionCleared();
                }
            }
            
           
            if (_previewCommand != null && selected != null && selected.Target is TransitionZoneView transitionZoneView)
            {
                _previewCommand.SetTarget(transitionZoneView);
                _gameStateContext.RunCommand(_previewCommand);
                ClearPreview();
                return;
            }
            _selected = selected;
            //_selectedTargetView.Set(selected);

            if (_player != null)
            {
                var targetingInfo = new TargetingInfo(_player, selected != null ? selected.Target : null);
                bool FilterInteractions(InteractionData interaction)
                {
                    if (interaction.IsValidTarget(targetingInfo))
                    {
                        return true;
                    }
                    return false;
                }
                var interactions = _gameStateContext.GetInteractions(FilterInteractions);
                _interactionList.Set(interactions, targetingInfo);
            }
            
            if (_selected == null || _selected.Target == null)
            {
                ClearPreview();
                return;
            }
            if (_selected.Target is IEntityView<CharacterEntity> selectableCharacter)
            {
                var instance = selectableCharacter.Entity;
                if (instance == null)
                {
                    _characterSelectPreview.Clear();
                    _playerInfoView.Set(_player);
                    return;
                }
                ReactionData lastReaction = null;
                _gameStateContext.LastInteraction?.Reactions.TryGetValue(selectableCharacter.Entity, out lastReaction);
                _characterSelectPreview.Set(instance, lastReaction);
                _playerInfoView.Clear();
            }
            else
            {
                _characterSelectPreview.Clear();
                _playerInfoView.Set(_player);
            }

            if (_selected.Target is PlayerInputHandler playerInputHandler)
            {
                if (_previewCommand == null)
                {
                    SetPreview(playerInputHandler.GetDefaultCommand());
                }
            }
            else
            {
                if (_previewCommand != null)
                {
                    _previewCommand.SetTarget(_selected.Target);
                }
            }
        }

        private void ClearPreview()
        {
            _rangeIndicatorView.Clear();
            _locationIndicatorView.Clear();
            _interactionList.Clear();
            _radiusIndicatorView.Clear();
            _characterSelectPreview.Clear();
            _playerInfoView.Set(_player);
            if (_player != null)
            {
                _player.HideAPDisplay();
            }
            _pointerSelectInputController.SetCanSelect(true);
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
                    if (_previewCommand is MoveInteractionData.Command moveCommand)
                    {
                        targetPos = moveCommand.TargetLocation;
                    }

                    if (_previewCommand.ShowRangeCircle)
                    {
                        if (!_rangeIndicatorView.gameObject.activeSelf)
                        {
                            _rangeIndicatorView.gameObject.SetActive(true);
                        }
                        _rangeIndicatorView.ShowDistance(sourcePos, _previewCommand.Range, _previewCommand.ValidateRange);
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
                        _radiusIndicatorView.ShowRadius(targetPos, radius, _previewCommand.ValidateRadiusTargets);
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