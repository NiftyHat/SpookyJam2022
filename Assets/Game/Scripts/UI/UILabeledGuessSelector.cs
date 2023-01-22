using System;
using Data.Monsters;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using TMPro;
using UI.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UILabeledGuessSelector : MonoBehaviour, IView<string>
    {
        [SerializeField] [NonNull] private UIGuessAnimation _guessAnimation;
        [SerializeField] [NonNull] private TextMeshProUGUI _label;
        [SerializeField] [NonNull] private Button _button;
        [SerializeField] [NonNull] private Image _icon;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private UIGuessAudio _guessAudio;

        protected Guess _guess;
        public event Action<Guess> OnValueChanged;

        public void Start()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            var nextGuess = GuessUtils.Next(_guess);
            SetGuess(nextGuess);
            OnValueChanged?.Invoke(_guess);
            if (_guessAudio != null)
            {
                _guessAudio.PlayStateAudio(nextGuess);
            }
        }

        public void SetGuess(Guess guess)
        {
            _guess = guess;
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
        
        public void Set(string label, Sprite icon)
        {
            if (label == null)
            {
                Clear();
            }
            _icon.sprite = icon;
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

        public void Set(string label, Sprite icon, Guess guess)
        {
            if (label == null)
            {
                Clear();
            }
            _label.SetText(label);
            _icon.sprite = icon;
            SetGuess(guess);
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}
