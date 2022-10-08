using NiftyScriptableSet;
using UnityEngine;
using UnityUtils;

namespace UI
{
    [CreateAssetMenu(fileName = "RingMenu", menuName = "RingMenu/Menu", order = 2)]
    public class RingMenuData : ScriptableSet<RingMenu.RingMenuItemData>, IFactory<RingMenu.RingMenuModel>
    {
        public RingMenu.RingMenuModel Create()
        {
            return new RingMenu.RingMenuModel(References);
        }

    }
}