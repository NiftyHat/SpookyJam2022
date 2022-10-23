using Data.Mask;
using Data.Style;
using NiftyFramework.UI;
using UnityEngine;

namespace Entity
{
    public class MaskEntity
    {
        protected Color _color;
        protected MaskData _mask;
        protected string _friendlyName;
        protected ColorStyleData _colorStyleData;

        public Color Color => _color;
        public MaskData MaskData => _mask;
        public ColorStyleData ColorStyleData => _colorStyleData;
        public string FriendlyName => _friendlyName;

        public MaskEntity(MaskData maskData, ColorStyleData colorStyleData)
        {
            _mask = maskData;
            _color = colorStyleData.Color;
            _colorStyleData = colorStyleData;
            _friendlyName = $"{colorStyleData.FriendlyName} {maskData.FriendlyName}";
        }

        public void Clear()
        {
            
        }

        public string ToString()
        {
            return _friendlyName;
        }
    }
}