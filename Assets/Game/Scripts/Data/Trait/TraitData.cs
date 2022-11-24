using NiftyFramework.Core;
using UI;
using UnityEngine;

namespace Data.Trait
{
    
    public class TraitData : ScriptableObject
    {
        [SerializeField] private string _friendlyName;
        [SerializeField][SpritePreview] private Sprite _icon;
        [SerializeField][TextArea] private string _description;
        [SerializeField] private bool _isEnabled;
        [SerializeField] private int _cardNumber;
        [SerializeField] private string _cardSuit;
        public string FriendlyName => _friendlyName;
        public Sprite Icon => _icon;
        public string Description => _description;
        public bool IsEnabled => _isEnabled;
        public int CardNumber => _cardNumber;
        public string CardSuit => _cardSuit;

        public ITooltip GetTooltip()
        {
            return new TooltipSimple(_icon, $"{_friendlyName} - {_description}");
        }
    }
}