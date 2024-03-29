using System;
using DG.Tweening;
using GameStats;
using Interactions.Commands;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using TMPro;
using UnityEngine;
using UnityUtils;

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
                if (command.APCostProvider != null)
                {
                    PreviewCost(command.APCostProvider.Value);
                    command.APCostProvider.OnChanged += HandleUpdatePreviewCost;
                }
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

            if (gameObject != null)
            {
                gameObject.TrySetActive(false);
            }
        }

        public void AnimateChange(int newValue, int oldValue)
        {
            float animDuration = 2.0f;
            void TweenUpdate(float value)
            {
                int displayValue = Mathf.Min(100, (int)value + 1);
                HandleActionPointsChanged(oldValue, displayValue);
            }

            if (gameObject != null && gameObject.TrySetActive(true))
            {
                Tween tween = DOTween.To(TweenUpdate, oldValue, newValue, animDuration).SetEase(Ease.OutCubic);
                tween.onComplete += Clear;
            }
            else
            {
                Clear();
            }
            
        }
    }
}
