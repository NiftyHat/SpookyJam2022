using NiftyFramework.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace Data.Audio
{
    [CreateAssetMenu(fileName = "AudioChannelData", menuName = "Game/Audio/AudioChannelData", order = 1)]
    public class AudioChannelData : ScriptableObject
    {
        public class VolumeProvider : ValueProvider<float>
        {
            public float GetDB()
            {
                if (Value > 0.01f)
                {
                    return Mathf.Log10(Value) * 20f;
                }
                return -80;
                
            }
        };

        [SerializeField] protected AudioMixer _audioMixer;
        [SerializeField] protected string _audioMixerParam;
        [SerializeField] protected string _displayName;

        public string DisplayName => _displayName;

        public readonly VolumeProvider Volume = new VolumeProvider();

        private void OnEnable()
        {
            TryLoadVolume();
            if (!_audioMixer.GetFloat(_audioMixerParam, out _))
            {
                Debug.LogError($"{nameof(AudioChannelData)} is targeting param '{_audioMixerParam}' which isn't exposed in the mixer.");
            }
        }
        
        private void UpdateMixer()
        {
            _audioMixer.SetFloat(_audioMixerParam, Volume.GetDB());
        }
        public void SetVolume(float value)
        {
            float clampedValue = Mathf.Clamp01(value);
            Volume.Value = Mathf.Clamp01(clampedValue);
            UpdateMixer();
            TrySaveVolume(clampedValue);
        }

        public bool TryLoadVolume()
        {
            string playerPrefsKey = GetVolumeKey();
            if (playerPrefsKey != null && PlayerPrefs.HasKey(playerPrefsKey))
            {
                Volume.Value = PlayerPrefs.GetFloat(playerPrefsKey);
                return true;
            }
            return false;
        }

        public bool TrySaveVolume(float value)
        {
            string playerPrefsKey = GetVolumeKey();
            if (playerPrefsKey != null)
            {
                PlayerPrefs.SetFloat(playerPrefsKey, Volume.Value);
                return true;
            }
            return false;
        }

        public string GetVolumeKey()
        {
            return $"AudioSettings.{_audioMixerParam}.Volume";
        }

        public string GetMuteKey()
        {
            return $"AudioSettings.{_audioMixerParam}.IsMute";
        }
        
    }
}