using System;
using Context;
using NiftyFramework.Core.Context;
using UnityEngine;

namespace UI.Audio
{
    public class AmbientAudioPlayer : MonoBehaviour
    {
        [Serializable]
        public struct AudioState
        {
            [Range(0f,1f)] public float MusicVolume;
            [Range(0f,1f)] public float OutdoorVolume;
        }
        
        [Serializable]
        public struct AmbientSource
        {
            [SerializeField] private AudioSource _audioSource;
            private float _defaultVolume;

            public void Init()
            {
                _defaultVolume = _audioSource.volume;
                _audioSource.Stop();
            }

            public void SetVolume(float percentage)
            {
                _audioSource.volume = _defaultVolume * percentage;
                if (percentage > 0 && !_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }
            }
        }

        [SerializeField] private AmbientSource _guestAmbience;
        [SerializeField] private AmbientSource _musicAmbiance;
        [SerializeField] private AmbientSource _outdoorsAmbience;

        protected AudioPlayerContext _service;

        private void Start()
        {
            _guestAmbience.Init();
            _musicAmbiance.Init();
            _outdoorsAmbience.Init();
            ContextService.Get<AudioPlayerContext>(HandleAudioContext);
        }

        private void HandleAudioContext(AudioPlayerContext service)
        {
            _service = service;
            _service.OnAmbientStateChange += HandleAmbientStateChange;
            _service.OnGuestVolumeChange += HandleGuestVolumeChange;
        }

        private void HandleGuestVolumeChange(float percentage)
        {
            _guestAmbience.SetVolume(percentage);
        }

        public void HandleAmbientStateChange(AudioState settings)
        {
            _musicAmbiance.SetVolume(settings.MusicVolume);
            _outdoorsAmbience.SetVolume(settings.OutdoorVolume); 
        }
    }
}