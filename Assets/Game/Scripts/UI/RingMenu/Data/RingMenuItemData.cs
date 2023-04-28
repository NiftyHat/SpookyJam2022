using System;
using Data;
using Data.Menu;
using UnityEngine;

namespace UI.RingMenu
{
    [CreateAssetMenu(fileName = "RingMenuItem", menuName = "RingMenu/MenuItem", order = 2)]
    public class RingMenuItemData : ScriptableObject, IRingMenuItem, IMenuItem
    {
        [SerializeField] private string _friendlyName;

        [SerializeField] private Sprite _icon;

        private string _description;
        private IIconViewData _selectionIcon;

        public string FriendlyName => _friendlyName;

        public Sprite Icon => _icon;

        public IIconViewData SelectionIcon => _selectionIcon;

        public event Action OnSelected;

        public string Description => _description;
        
    }
}
