using NiftyScriptableSet;
using UnityEngine;
using UnityUtils;

namespace UI
{
    [CreateAssetMenu(fileName = "RingMenu", menuName = "RingMenu/Menu", order = 2)]
    public class RingMenuData : ScriptableSet<RingMenuItemData>, IFactory<RingMenuModel>
    {
        [SerializeField] private RingMenuItemData[] _items;
        public RingMenuModel Create()
        {
            return new RingMenuModel(_items);
        }

    }
}