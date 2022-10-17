using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Screens
{
    public class AnimatedTimeLabelView : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI _text;

        private TimeSpan _timeSpan;
        private long _ticks;
        
        public void Set(TimeSpan newTime, float animDuration)
        {
            if (animDuration > 0)
            {
                _ticks = _timeSpan.Ticks;
                DOTween.To(() => _ticks, x =>
                {
                    _ticks = x;
                    Set(_ticks);
                }, newTime.Ticks, animDuration);
            }
            else
            {
                _timeSpan = newTime;
                _ticks = newTime.Ticks;
            }
            
        }

        public void Set(long ticks)
        {
            Set(TimeSpan.FromTicks(ticks));
        }
        
        public void Set(TimeSpan startTime, TimeSpan endTime, float animDuration = 0)
        {
            if (animDuration > 0)
            {
                _ticks = startTime.Ticks;
                Tween tween = DOTween.To(() => _ticks, x =>
                {
                    _ticks = x;
                    Set(_ticks);
                }, endTime.Ticks, animDuration);
                tween.onComplete += () =>
                {
                    Set(endTime);
                };
            }
            else
            {
                Set(endTime);
            }
        }

        public void Set(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
            _text.text = timeSpan.ToString(@"hh\:mm") + "pm";
        }
        
    }
}