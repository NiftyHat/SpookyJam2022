using UnityEngine;

namespace Data.Mask
{
    public class MaskData : ScriptableObject
    {
        [SerializeField] private string _friendlyName;
        [SerializeField] private Sprite _worldSprite;
        [SerializeField] private Sprite _cardSprite;
        [SerializeField] private Sprite _iconSprite;

        public string FriendlyName => _friendlyName;
        public Sprite WorldSprite => _worldSprite;
        public Sprite CardSprite => _cardSprite;
        public Sprite IconSprite => _iconSprite;
    }
}