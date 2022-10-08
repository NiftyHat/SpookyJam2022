using System.Collections.Generic;
using System.Linq;

namespace UI.RingMenu
{
    public class RingMenuModel
    {
    private IList<IRingMenuItem> _items;

    public RingMenuModel(IEnumerable<RingMenuItemData> itemData)
    {
        _items = itemData.Cast<IRingMenuItem>().ToList();
    }

    public RingMenuModel(IList<IRingMenuItem> items)
    {
        _items = items;
    }

    public int Count
    {
        get => _items?.Count ?? 0;
    }
    }
}