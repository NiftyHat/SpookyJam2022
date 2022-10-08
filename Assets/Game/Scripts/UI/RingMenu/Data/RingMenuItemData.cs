using Data.Menu;
using UnityEngine;

namespace UI.RingMenu
{
    [CreateAssetMenu(fileName = "RingMenuItem", menuName = "RingMenu/MenuItem", order = 2)]
    public class RingMenuItemData : ScriptableObject, IRingMenuItem, IMenuItem
    {
        [SerializeField] private string _friendlyName;

        [SerializeField] private Sprite _icon;
        
        private RingMenu.RingMenuModel _subMenu;
        
        private string _description;

        public string FriendlyName => _friendlyName;

        public Sprite Icon => _icon;

        public string Description => _description;

        public RingMenu.RingMenuModel SubMenu => _subMenu;
    }
}
