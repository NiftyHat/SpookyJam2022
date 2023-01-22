using NiftyFramework.Core.Utils;
using UI.Audio.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Audio
{
    public class ButtonAudio : MonoBehaviour
    {
        [SerializeField] [NonNull] private Button _button;
        [SerializeField][NonNull] private AudioSource _audioSource;
        
        [SerializeField] private AudioButtonData _data;

        public void Start()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            _audioSource.PlayOneShot(_data.Click);
        }
    }
}