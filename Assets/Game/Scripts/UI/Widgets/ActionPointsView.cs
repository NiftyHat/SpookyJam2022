using GameStats;
using Interactions.Commands;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using TMPro;
using UnityEngine;

namespace UI.Widgets
{
    public class ActionPointsView : MonoBehaviour, IView<GameStat>, IView<GameStat, InteractionCommand>
    {
        [SerializeField][NonNull] private TextMeshPro _textValueDisplay;
        [SerializeField] protected Color _previewNegativeValue;
        [SerializeField] protected Color _previewOverMax;
        [SerializeField] protected Color _defaultColor;
        protected GameStat _gameStat;

        public void Start()
        {
            _defaultColor = _textValueDisplay.color;
        }

        public void Set(GameStat gameStat)
        {
            if (_gameStat != null)
            {
                _gameStat.OnChanged -= HandleActionPointsChanged;
            }
            _gameStat = gameStat;
            if (_gameStat != null)
            {
                _textValueDisplay.color = _defaultColor;
                _gameStat.OnChanged += HandleActionPointsChanged;
                _textValueDisplay.text = gameStat.Value + " " + _gameStat.Abbreviation;
                gameObject.SetActive(true);
            }
        }

        public void PreviewCost(int cost)
        {
            PreviewChange(-cost);
        }

        public void PreviewChange(int change)
        {
            int previewAmount = _gameStat.Value + change;
            if (previewAmount > _gameStat.Max)
            {
                _textValueDisplay.color = _previewOverMax;
            }
            else if (previewAmount < 0)
            {
                _textValueDisplay.color = _previewNegativeValue;
            }
            else
            {
                _textValueDisplay.color = _defaultColor;
            }
            _textValueDisplay.text = previewAmount + " " + _gameStat.Abbreviation;
        }

        private void HandleActionPointsChanged(int oldValue, int newValue)
        {
            _textValueDisplay.text = newValue + " " + _gameStat.Abbreviation;
        }

        public void Set(GameStat gameStat, InteractionCommand command)
        {
            Set(gameStat);
            if (command != null)
            {
                PreviewCost(command.APCostProvider.Value);
                command.APCostProvider.OnChanged += HandleUpdatePreviewCost;
            }
        }

        private void HandleUpdatePreviewCost(int oldValue, int newValue)
        {
            PreviewCost(newValue);
        }

        public void Clear()
        {
            if (_gameStat != null)
            {
                _gameStat.OnChanged -= HandleActionPointsChanged;
            }

            if (_textValueDisplay != null)
            {
                _textValueDisplay.color = _defaultColor;
                _textValueDisplay.text = "";
            }

            gameObject.SetActive(false);
        }
    }
}
