using UnityEngine;

namespace Data.Trait
{
    
    public class TraitData : ScriptableObject
    {
        [SerializeField] private string _friendlyName;
        [SerializeField] private Sprite _icon;
        [SerializeField][TextArea] private string _description;
        public string FriendlyName => _friendlyName;
        public Sprite Icon => _icon;
        public string Description => _description;
    }
}