using Data;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cards
{
    public class UICardBorderView : MonoBehaviour, IView<Guess>
    {
        [SerializeField] private Image _image;
        [SerializeField][NonNull] private GuessStyleData _guessStyle;

        private Color? _defaultColour;

        public void Awake()
        {
            _defaultColour = _image.color;
        }
        
        public void Set(Guess enumGuess)
        {
            if (_defaultColour == null)
            {
                _defaultColour = _image.color;
            }
            if (_guessStyle != null && _guessStyle.TryGet(enumGuess, out var appliedStyle))
            {
                _image.color = appliedStyle.Color;
            }
            else
            {
                if (_defaultColour != null)
                {
                    _image.color = _defaultColour.Value;
                }
            }
            
        }

        public void Clear()
        {
            if (_defaultColour != null)
            {
                _image.color = _defaultColour.Value;
            }
        }
    }
}