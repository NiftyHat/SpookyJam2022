using NiftyScriptableSet;
using UnityEngine;
using UnityUtils;

namespace UI
{
    [CreateAssetMenu(fileName = "RingMenu", menuName = "RingMenu/Menu", order = 2)]
    public class RingMenuData : ScriptableSet<RingMenuItemData>, IFactory<RingMenuModel>
    {
        public RingMenuModel Create()
        {
            return new RingMenuModel(References);
        }

    }
}