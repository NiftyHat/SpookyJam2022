using UnityEngine;
using UnityUtils;

namespace UI.ContextMenu
{
    public interface IContextItem
    {
        
    }
    
    public class ContextMenuData
    {
        public class ItemData : IContextItem
        {
            public readonly Sprite Icon;
            public string FriendlyName;
            public string Description;
        }
    }
}