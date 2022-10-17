using System;
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

        public string FriendlyName => _friendlyName;

        public Sprite Icon => _icon;
        public event Action OnSelected;

        public string Description => _description;
        
    }
}
