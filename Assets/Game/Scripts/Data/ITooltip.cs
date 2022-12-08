using UnityEngine;

namespace Data
{
    public interface ITooltip
    {
        public IIconViewData GetIcon();
        public string GetCopy();
        void SetTarget(RectTransform transform);
    }
}