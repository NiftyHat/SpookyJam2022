using System.Collections.Generic;
using System.Linq;

namespace UI.RingMenu
{
    public interface IRingMenuViewData
    {
        public IList<IRingMenuItem> Items { get; }
        public int Count { get; }
        
    }
    
    public class RingMenuViewData
    {
        private IList<IRingMenuItem> _items;

        public RingMenuViewData(IEnumerable<RingMenuItemData> itemData)
        {
            _items = itemData.Cast<IRingMenuItem>().ToList();
        }

        public RingMenuViewData(IList<IRingMenuItem> items)
        {
            _items = items;
        }

        public int Count
        {
            get => _items?.Count ?? 0;
        }
    }
}