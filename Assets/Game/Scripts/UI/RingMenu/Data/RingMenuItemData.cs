using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "RingMenuItem", menuName = "RingMenu/MenuItem", order = 2)]
    public class RingMenuItemData : ScriptableObject, IRingMenuItem
    {
        [SerializeField] private string _friendlyName;

        [SerializeField] private Sprite _icon;
        
        private RingMenuModel _subMenu;

        public string FriendlyName => _friendlyName;

        public Sprite Icon => _icon;

        public RingMenuModel SubMenu => _subMenu;
    }
}
