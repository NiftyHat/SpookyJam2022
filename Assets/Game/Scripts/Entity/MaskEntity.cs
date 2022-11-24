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
        protected int _cardValue;

        public Color Color => _color;
        public MaskData MaskData => _mask;
        public ColorStyleData ColorStyleData => _colorStyleData;
        public string FriendlyName => _friendlyName;

        public int CardValue => _cardValue;

        public MaskEntity(MaskData maskData, ColorStyleData colorStyleData, int cardValue)
        {
            _mask = maskData;
            _color = colorStyleData.Color;
            _colorStyleData = colorStyleData;
            _friendlyName = $"{colorStyleData.FriendlyName} {maskData.FriendlyName}";
            _cardValue = cardValue;
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