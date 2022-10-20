using UnityEngine;

namespace Data.Trait
{
    
    public class TraitData : ScriptableObject
    {
        [SerializeField] private string _friendlyName;
        [SerializeField] private Sprite _icon;
        public string FriendlyName => _friendlyName;
        public Sprite Icon => _icon;
    }
}