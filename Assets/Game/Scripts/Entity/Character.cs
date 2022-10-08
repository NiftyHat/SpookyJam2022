using UI.ContextMenu;
using UnityEngine;

namespace Entity
{
    public class Character : MonoBehaviour
    {
        public UnitInputHandler _unitInputHandler;
        [SerializeField] protected GameObject _goSelectedIndicator;
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        private object _handleContextMenuRequest;
        private IContextMenu _contextMenu;

        public void Start()
        {
            if (_unitInputHandler == null)
            {
                _unitInputHandler = GetComponent<UnitInputHandler>();
            }

            if (_unitInputHandler != null)
            {
                _unitInputHandler.OnSelectChange += HandleSelectedChanged;
                _unitInputHandler.OnContextMenuRequest += HandleContextMenuRequest;
            }

            if (_goSelectedIndicator != null)
            {
                _goSelectedIndicator.SetActive(false);
            }
        }

        public virtual IContextMenu CreateContextMenu()
        {
            return null;
        }

        private bool HandleContextMenuRequest(out IContextMenu contextMenu)
        {
            contextMenu = _contextMenu;
            return contextMenu != null;
        }

        private void HandleSelectedChanged(bool isSelected)
        {
            if (_goSelectedIndicator != null)
            {
                _goSelectedIndicator.SetActive(isSelected);
            }
        }
    }
}