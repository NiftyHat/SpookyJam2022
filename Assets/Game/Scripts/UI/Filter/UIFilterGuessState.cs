using System;
using System.Collections.Generic;
using Data.Menu;
using NiftyFramework.Core;
using UnityEngine;

namespace UI.Filter
{
    public class UIFilterGuessState: UIFilterButtonSetView<UIFilterGuessState.ItemData>
    {
        [Serializable]
        public class ItemData : IMenuItemProvider
        {
            [SerializeField] private MenuItemData _menuItem;

            [Serializable] public class MenuItemData : IMenuItem
            {
                [SerializeField] private string _friendlyName;
                [SerializeField][SpritePreview] private Sprite _icon;
                [SerializeField] private string _description;

                public string FriendlyName => _friendlyName;

                public Sprite Icon => _icon;

                public string Description => _description;
            }

            public IMenuItem MenuItem => _menuItem;
            [SerializeField] protected Guess _guess;
            
            public Guess Guess => _guess;
        }

        [SerializeField] public List<ItemData> _data;

        public void Start()
        {
            Set(_data);
            SetLabel(_defaultTitle);
        }
        
    }
}