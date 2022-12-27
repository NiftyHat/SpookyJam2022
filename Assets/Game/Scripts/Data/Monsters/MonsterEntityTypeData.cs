using System;
using Data.Menu;
using Data.Trait;
using Entity;
using Generators;
using NiftyFramework.Core;
using UI;
using UnityEngine;

namespace Data.Monsters
{
    
    public class MonsterEntityTypeData : ScriptableObject, IRevealProvider, IUniqueEntityType, IMenuItemProvider
    {
        [Serializable]
        public struct MenuItemData : IMenuItem
        {
            [SerializeField] private string _friendlyName;
            [SerializeField][SpritePreview] private Sprite _icon;
            [SerializeField][TextArea] private string _description;

            public string FriendlyName => _friendlyName;
            public Sprite Icon => _icon;
            
            public string Description => _description;
            
        }
        
        [SerializeField] private string _friendlyName;
        [SerializeField] private Sprite _revealSprite;
        [SerializeField] private TraitData[] _preferredTraits;
        [SerializeField] private ScheduleGenerator _scheduleGenerator;
        [SerializeField] private MenuItemData _menuItem;
        
        public Sprite RevealSprite => _revealSprite;
        public string FriendlyName => _friendlyName;
        public Sprite Icon => _menuItem.Icon;
        public TraitData[] PreferredTraits => _preferredTraits;
        public ScheduleGenerator ScheduleGenerator => _scheduleGenerator;
        public IMenuItem MenuItem => _menuItem;
    }
}