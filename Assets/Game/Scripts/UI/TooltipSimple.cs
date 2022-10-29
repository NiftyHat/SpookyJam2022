using Data;
using UnityEngine;

namespace UI
{
    public class TooltipSimple : ITooltip
    {
        public readonly IconViewData _iconViewData;
        public readonly string _copy;
        public TooltipSimple(Sprite sprite, string copy)
        {
            _iconViewData = new IconViewData(sprite);
            _copy = copy;
        }
        
        public IIconViewData GetIcon()
        {
            return _iconViewData;
        }

        public string GetCopy()
        {
            return _copy;
        }
    }
}