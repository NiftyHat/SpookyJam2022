using System;
using Data.Monsters;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UILabeledGuessSelector : MonoBehaviour, IView<string>
    {
        [SerializeField] [NonNull] private UIGuessAnimation _guessAnimation;
        [SerializeField] [NonNull] private TextMeshProUGUI _label;
        [SerializeField] [NonNull] private Button _button;
        [SerializeField] private CanvasGroup _canvasGroup;

        protected Guess _guess;
        public event Action<Guess> OnValueChanged;
        protected bool _isEliminated;

        public void Start()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void SetEliminated(bool isEliminated)
        {
            _isEliminated = isEliminated;
            if (_canvasGroup != null)
            {
                if (isEliminated)
                {
                    _canvasGroup.alpha = 0.5f;
                }
                else
                {
                    _canvasGroup.alpha = 1.0f;
                }
            }
        }

        private void HandleButtonClick()
        {
            _guess = GuessUtils.Next(_guess);
            SetGuess(_guess);
            OnValueChanged?.Invoke(_guess);
        }

        public void SetGuess(Guess guess)
        {
            if (_guessAnimation != null)
            {
                _guessAnimation.Set(guess);
            }
        }
    
        public void Set(string label)
        {
            if (label == null)
            {
                Clear();
            }
            _label.SetText(label);
        }
    
        public void Set(string label, Guess guess)
        {
            if (label == null)
            {
                Clear();
            }
            _label.SetText(label);
            SetGuess(guess);
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}
