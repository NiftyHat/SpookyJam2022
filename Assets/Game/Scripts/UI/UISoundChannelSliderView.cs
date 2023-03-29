using Data.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UISoundChannelSliderView : MonoBehaviour
    {
        [SerializeField] protected AudioChannelData _audioChannel;
        [SerializeField] protected Slider _slider;
        [SerializeField] protected TextMeshProUGUI _textTitle;
        [SerializeField] protected TextMeshProUGUI _textVolumeNumber;

        // Start is called before the first frame update
        void Start()
        {
            _slider.onValueChanged.AddListener(HandleSliderChanged);
            _textTitle.SetText(_audioChannel.DisplayName);
            _slider.value = _audioChannel.Volume.Value;
            HandleVolumeChanged(_slider.value, _slider.value);
        }

        private void OnEnable()
        {
            _audioChannel.Volume.OnChanged += HandleVolumeChanged;
        }

        private void OnDisable()
        {
            _audioChannel.Volume.OnChanged -= HandleVolumeChanged;
        }

        private void HandleVolumeChanged(float newValue, float oldValue)
        {
            int displayValue = Mathf.RoundToInt(newValue * 100);
            if (displayValue == 0)
            {
                _textVolumeNumber.text = "00";
            }
            else
            {
                _textVolumeNumber.text = displayValue.ToString();   
            }

            if (_slider.value != newValue)
            {
                _slider.SetValueWithoutNotify(newValue);
            }
        }

        private void HandleSliderChanged(float value)
        {
            _audioChannel.SetVolume(value);
        }
    }
}
