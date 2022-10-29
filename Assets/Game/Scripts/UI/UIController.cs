using Data.Interactions;
using Entity;
using Interactions;
using NiftyFramework.Core.Utils;
using TouchInput.UnitControl;
using UI.Targeting;
using UnityEngine;


namespace UI
{
    public class UIController : MonoBehaviour
    {
        private UnitInputHandler _mainTargetUnit;
        [SerializeField] private LocationIndicatorView _locationIndicatorView;
        [SerializeField] private UnitInputController _unitInputController;
        [SerializeField] [NonNull] private RangeIndicatorView _rangeIndicatorView;
        [SerializeField] [NonNull] private UISelectedTargetView _selectedTargetView;

        public void Start()
        {
            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
            }
            _unitInputController.OnUnitSelected += HandleUnitSelected;
            _unitInputController.OnGroundSelected += HandleGroundSelected;
        }


        private void HandleGroundSelected(MovementPlaneView groundPlane, RaycastHit raycast)
        {
            if (_mainTargetUnit != null && _mainTargetUnit.TryGetInteraction(out var interaction))
            {
                interaction.ConfirmInput(raycast);
                if (_rangeIndicatorView != null)
                {
                    _rangeIndicatorView.Clear();
                }
            }
        }

        private void HandleUnitSelected(UnitInputHandler unit)
        {
            SetSelectedUnit(unit);
        }

        public void SetSelectedUnit(UnitInputHandler selectedInputHandler)
        {
            _mainTargetUnit = selectedInputHandler;
            if (_mainTargetUnit is ITargetable<CharacterEntity> targetableCharacter)
            {
                var target = targetableCharacter.GetTarget();
                _selectedTargetView.Set(target);
            }
            else
            {
                _selectedTargetView.Clear();
            }

            if (_mainTargetUnit == null || _mainTargetUnit.TryGetInteraction(out _) == false)
            {
                if (_locationIndicatorView != null)
                {
                    _locationIndicatorView.gameObject.SetActive(false);
                }

                if (_rangeIndicatorView != null)
                {
                    _rangeIndicatorView.Clear();
                }
            }
        }

        protected void Update()
        {
            if (_mainTargetUnit != null &&
                _mainTargetUnit.TryGetInteraction(out IInteraction interaction))
            {
                if (interaction.TargetPosition.HasValue && interaction.Range > 0)
                {
                    _locationIndicatorView.gameObject.SetActive(true);
                    if (!_locationIndicatorView.gameObject.activeSelf)
                    {
                        _locationIndicatorView.gameObject.SetActive(true);
                    }

                    _locationIndicatorView.ShowDistance(interaction.Source.GetWorldPosition(),
                        interaction.TargetPosition.Value, interaction.ValidateRange);

                    if (interaction.IsState(InteractionData.State.Selected))
                    {
                        if (!_rangeIndicatorView.gameObject.activeSelf)
                        {
                            _rangeIndicatorView.gameObject.SetActive(true);
                        }

                        float maxRange = interaction.GetMaxRange();
                        _rangeIndicatorView.ShowDistance(interaction.Source.GetWorldPosition(),
                            interaction.TargetPosition.Value, maxRange, interaction.ValidateRange);
                    }
                   
                }
                else
                {
                    if (_locationIndicatorView.gameObject.activeSelf)
                    {
                        _locationIndicatorView.gameObject.SetActive(false);
                    }

                    if (_rangeIndicatorView.gameObject.activeSelf)
                    {
                        _rangeIndicatorView.Clear();
                    }
                }
            }
        }
    }
}