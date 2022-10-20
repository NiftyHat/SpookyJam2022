using UnityEngine;

namespace Data.Area
{
    public class AreaData : ScriptableObject
    {
        [SerializeField] private string _friendlyName;
        [SerializeField] [TextArea] private string _description;
        [SerializeField] private Sprite _icon;

        public string GetFriendlyName()
        {
            return _friendlyName;
        }

        public Sprite GetSprite()
        {
            return _icon;
        }
    }
}