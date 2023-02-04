using Data.TextSequence;
using Febucci.UI;
using TextSequence.Data;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TextAnimatableCopyWidget : MonoBehaviour
    {
        public TextSequenceSetText _data;
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private TextAnimatorPlayer _animatorPlayer;

        private TextSequenceSetText _lastData;
        
        // Start is called before the first frame update
        void Start()
        {
            if (_animatorPlayer == null && _textMeshPro != null)
            {
                _animatorPlayer = _textMeshPro.GetComponent<TextAnimatorPlayer>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_lastData != _data)
            {
                _lastData = _data;
                if (_animatorPlayer != null)
                {
                    _animatorPlayer.ShowText(_data.Copy);
                    _animatorPlayer.StartShowingText();
                }
                else
                {
                    _textMeshPro.SetText(_data.Copy);
                }
            }
        }
    }
}
