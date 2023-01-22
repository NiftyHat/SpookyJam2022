using UnityEngine;

namespace UI.Audio
{
    public class UIAudioSelection : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private AudioClip _clipSelect;
        [SerializeField] private AudioClip _clipGroundSelect;
        [SerializeField] private AudioClip _clearClear;

        public void PlayGroundSelected()
        {
            _audioSource.PlayOneShot(_clipGroundSelect);
        }

        public void PlaySelectionCleared()
        {
            _audioSource.PlayOneShot(_clearClear);
        }

        public void PlaySelected()
        {
            _audioSource.PlayOneShot(_clipSelect);
        }
    }
}