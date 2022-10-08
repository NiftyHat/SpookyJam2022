using UnityEngine;

namespace UI.ContextMenu
{
    public interface IContextMenu
    {
        public void Open(Vector2 screenPosition);

        public void Close();
    }
    
}