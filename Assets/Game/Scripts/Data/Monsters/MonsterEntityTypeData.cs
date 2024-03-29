using System;
using Data.Menu;
using Data.TextSequence;
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
            private IIconViewData _selectionIcon;

            public string FriendlyName => _friendlyName;
            public Sprite Icon => _icon;

            public IIconViewData SelectionIcon => _selectionIcon;

            public string Description => _description;

        }
        
        [SerializeField] private string _friendlyName;
        [SerializeField] private Sprite _revealSprite;
        [SerializeField] private TraitData[] _preferredTraits;
        [SerializeField] private ScheduleGenerator _scheduleGenerator;
        [SerializeField] private MenuItemData _menuItem;
        [SerializeField] private bool _hasCopyMaskAbility;
        [SerializeField] private bool _hasCopyScheduleAbility;
        [SerializeField] private bool _hasFollowGuestAbility;
        [SerializeField] private TextSequenceSetData _revealDialogue;
        [SerializeField] private ColoredTextData _copyWin;
        [SerializeField] private int _bookPage;
        
        public Sprite RevealSprite => _revealSprite;
        public string FriendlyName => _friendlyName;
        public Sprite Icon => _menuItem.Icon;
        public TraitData[] PreferredTraits => _preferredTraits;
        public ScheduleGenerator ScheduleGenerator => _scheduleGenerator;
        public IMenuItem MenuItem => _menuItem;

        public TextSequenceSetData RevealDialogue => _revealDialogue;
        public ColoredTextData CopyWin => _copyWin;

        public bool HasCopyMaskAbility => _hasCopyMaskAbility;
        public bool HasCopyScheduleAbility => _hasCopyScheduleAbility;
        public bool HasFollowGuestAbility => _hasFollowGuestAbility;
        
        public int BookPage => _bookPage;
    }
}