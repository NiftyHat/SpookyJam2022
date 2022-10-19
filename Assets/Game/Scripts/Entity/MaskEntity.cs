using Data.Mask;
using Data.Style;
using NiftyFramework.UI;
using UnityEngine;

namespace Entity
{
    public class MaskEntity : IView<MaskData, ColorStyleData>
    {
        protected Color _color;
        protected MaskData _mask;
        protected string _friendlyName;

        public Color Color => _color;
        public MaskData MaskData => _mask;
        public string FriendlyName => _friendlyName;

        public void Set(MaskData maskData, ColorStyleData colorStyleData)
        {
            _mask = maskData;
            _color = colorStyleData.Color;
            _friendlyName = $"{colorStyleData.FriendlyName} {maskData.FriendlyName}";
        }

        public void Clear()
        {
            
        }
    }
}