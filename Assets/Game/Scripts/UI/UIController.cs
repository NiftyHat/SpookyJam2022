using Context;
using Data;
using Entity;
using Interactions;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using TouchInput.UnitControl;
using UI.ContextMenu;
using UI.Targeting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        private UnitInputHandler _mainTargetUnit;
        [SerializeField] private LocationIndicatorView _locationIndicatorView;
        [SerializeField] private UnitInputController _unitInputController;
        [SerializeField][NonNull] private UISelectedTargetView _selectedTargetView;
        
        [SerializeField] [NonNull] private EndTurnButtonWidget _endTurnButton;

        private GameStateContext _gameStateContext;

        public void Start()
        {
            _unitInputController.OnUnitSelected += HandleUnitSelected;
            _unitInputController.OnGroundSelected += HandleGroundSelected;
        }

        
        private void HandleGroundSelected(MovementPlaneView groundPlane, RaycastHit raycast)
        {
            if (_mainTargetUnit != null && _mainTargetUnit.TryGetInteraction(out var interaction))
            {
                interaction.ConfirmInput(raycast);
            }
        }

        private void HandleUnitSelected(UnitInputHandler unit)
        {
            SetSelectedUnit(unit);
        }

        public void SetSelectedUnit(UnitInputHandler unitInputHandler)
        {
            _mainTargetUnit = unitInputHandler;
            if (_mainTargetUnit is ITargetable<CharacterEntity> targetableCharacter)
            {
                _selectedTargetView.Set(targetableCharacter.GetTarget());
            }
            if (_mainTargetUnit == null || _mainTargetUnit.TryGetInteraction(out _) == false)
            {
                if (_locationIndicatorView != null)
                {
                    _locationIndicatorView.gameObject.SetActive(false);
                }
            }
        }
        
        protected void Update()
        {
            if (_mainTargetUnit != null && 
                _mainTargetUnit.TryGetInteraction(out IInteraction interaction)) 
            {
                if (interaction.TargetPosition.HasValue && interaction.Range > 0 )
                {
                    _locationIndicatorView.gameObject.SetActive(true);
                    if (!_locationIndicatorView.gameObject.activeSelf)
                    {
                        _locationIndicatorView.gameObject.SetActive(true);
                    }
                    _locationIndicatorView.ShowDistance(interaction.Source.GetWorldPosition(), interaction.TargetPosition.Value, interaction.ValidateRange);
                }
                else
                {
                    if (_locationIndicatorView.gameObject.activeSelf)
                    {
                        _locationIndicatorView.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}