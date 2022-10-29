using NiftyFramework.Core.Data;
using UnityEngine;

namespace Data
{
    public class IconViewData : IIconViewData
    {
        private Sprite _sprite;
        private Optional<Color> _tint;

        public Sprite Sprite => _sprite;

        public Optional<Color> Tint => _tint;

        public IconViewData(Sprite sprite, Color tint)
        {
            _sprite = sprite;
            _tint = new Optional<Color>(tint);
        }
        
        public IconViewData(Sprite sprite)
        {
            _sprite = sprite;
        }
    }
}