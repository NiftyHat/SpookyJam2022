using NiftyFramework.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class ButtonAudio : MonoBehaviour
    {
        [SerializeField] [NonNull] private Button _button;
        [SerializeField][NonNull] private AudioSource _audioSource;
        
        [SerializeField] private AudioClip _click;

        public void Start()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            _audioSource.PlayOneShot(_click);
        }
    }
}