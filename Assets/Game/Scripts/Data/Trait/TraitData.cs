using NiftyFramework.Core;
using UnityEngine;

namespace Data.Trait
{
    
    public class TraitData : ScriptableObject
    {
        [SerializeField] private string _friendlyName;
        [SerializeField][SpritePreview] private Sprite _icon;
        [SerializeField][TextArea] private string _description;
        [SerializeField] private bool _isEnabled;
        public string FriendlyName => _friendlyName;
        public Sprite Icon => _icon;
        public string Description => _description;
        public bool IsEnabled => _isEnabled;
    }
}