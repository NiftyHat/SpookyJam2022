using UnityEngine;

namespace Data.Style
{
    public class ColorStyleData : ScriptableObject
    {
        [SerializeField] protected Color _color;
        [SerializeField] protected string _friendlyName;

        public Color Color => _color;
        public string FriendlyName => _friendlyName;
    }
}