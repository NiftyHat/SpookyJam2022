using UnityEngine;

namespace UI.RingMenu
{
    public class RingMenuItemModel
    {
        public string FriendlyName;
        public Sprite Icon;
        public RingMenuModel SubMenu;
    }

    public interface IRingMenuItem
    {
        string FriendlyName { get; }
        Sprite Icon { get; }
        RingMenuModel SubMenu { get; }
    }
}
