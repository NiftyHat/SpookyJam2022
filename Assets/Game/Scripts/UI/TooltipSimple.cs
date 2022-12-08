using System;
using Data;
using UnityEngine;

namespace UI
{
    public class TooltipSimple : ITooltip
    {
        public readonly IconViewData IconViewData;
        public readonly string BodyCopy;
        protected RectTransform _target;
        public RectTransform Target => _target;
        public event Action<RectTransform> OnTargetChange;
        public TooltipSimple(Sprite sprite, string bodyCopy)
        {
            IconViewData = new IconViewData(sprite);
            BodyCopy = bodyCopy;
        }

        public void SetTarget(RectTransform target)
        {
            _target = target;
            OnTargetChange?.Invoke(target);
        }
        
        public IIconViewData GetIcon()
        {
            return IconViewData;
        }

        public string GetCopy()
        {
            return BodyCopy;
        }
    }
}