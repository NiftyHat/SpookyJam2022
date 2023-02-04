using System;
using System.Collections.Generic;
using Data.TextSequence;
using Entity;
using Febucci.UI;
using NiftyFramework.UI;
using TextSequence.Data;
using TMPro;
using UnityEngine;

namespace TextSequence
{
    public class TextSequencePlayerWidget : MonoBehaviour, IView<CharacterName>, IView<CharacterName, TextSequenceSetData>, IView<TextSequenceSetData>
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private TextAnimatorPlayer _animatorPlayer;
        [SerializeField] private TextSequenceSetData _initData;
        [SerializeField] private bool _isPlaying;
        [SerializeField] private NextPromptWidget _nextPromptWidget;
        [SerializeField] private TextMeshProUGUI _labelCharacterName;

        private List<TextSequenceItem> _sequence;
        private int _position = -1;
        private bool _canSkip;
        private bool _wasPlaying;

        public event Action OnComplete;
        public event Action OnClear;

        private void Start()
        {
            _nextPromptWidget.OnInput += HandleNextInput;
            //_initData = null;
        }

        public void Play()
        {
            Goto(0);
        }

        private void Advance()
        {
            _nextPromptWidget.SetVisible(false);
            Goto(_position + 1);
        }

        private void Goto(int position)
        {
            _position = position;
            TextSequenceItem next = null;
            if (_position < _sequence.Count)
            {
                next = _sequence[_position];
            }

            if (next == null)
            {
                _canSkip = false;
                _animatorPlayer.onTextShowed.RemoveListener(HandleTextAnimationComplete);
                OnComplete?.Invoke();
            }
            if (next is TextSequenceSetText setText)
            {
                _canSkip = true;
                _animatorPlayer.ShowText(setText.Copy);
                _animatorPlayer.StartShowingText();
                _animatorPlayer.onTextShowed.AddListener(HandleTextAnimationComplete);
            }
        }

        private void Update()
        {
            if (_wasPlaying != _isPlaying)
            {
                _wasPlaying = _isPlaying;
                if (_isPlaying)
                {
                    Play();
                }
                else
                {
                    _animatorPlayer.StopShowingText();
                }
            }
       
        }
    
        private void HandleNextInput()
        {
            if (_isPlaying)
            {
                if (_canSkip)
                {
                    _nextPromptWidget.SetVisible(false);
                    _animatorPlayer.SkipTypewriter();
                    _canSkip = false;
                }
                else
                {
                
                    Advance();
                }
            }
        }
    

        private void HandleTextAnimationComplete()
        {
            _nextPromptWidget.SetVisible(true);
            _canSkip = false;
        }

        public void Set(CharacterName characterName)
        {
            _labelCharacterName.SetText(characterName.Full);
        }

        public void Set(CharacterName characterName, TextSequenceSetData sequence)
        {
            Set(characterName);
            Set(sequence);
        }

        public void Set(TextSequenceSetData sequence)
        {
            _sequence = sequence.References;
            //Goto(0);
        }

        public void Clear()
        {
            OnClear?.Invoke();
        }
    }
}
