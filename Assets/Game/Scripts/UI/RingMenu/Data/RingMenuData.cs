using System.Collections.Generic;
using System.Linq;
using NiftyScriptableSet;
using UnityEngine;

namespace UI.RingMenu
{
    [CreateAssetMenu(fileName = "RingMenu", menuName = "RingMenu/Menu", order = 2)]
    public class RingMenuData : ScriptableSet<RingMenuItemData>, IRingMenuViewData, ISerializationCallbackReceiver
    {
        private IList<IRingMenuItem> _items;
        public IList<IRingMenuItem> Items => _items;
        public int Count => _items?.Count ?? 0;
        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            _items = References.Cast<IRingMenuItem>().ToList();
        }
    }
}