using System.Collections.Generic;
using System.Linq;

namespace UI
{
    public class RingMenuModel
    {
        private List<IRingMenuItem> _items;
        public RingMenuModel(RingMenuItemData[] itemData)
        {
            _items = itemData.Cast<IRingMenuItem>().ToList();
        }

        public int Count
        {
            get => _items?.Count ?? 0;
        }
    }
}