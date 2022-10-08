using TouchInput.UnitControl;
using UI.ContextMenu;
using UnityEngine;

public class UnitInputHandler : MonoBehaviour
{
    public delegate void SelectStateChanged(bool isSelected);
    public delegate bool ContextMenuRequested(out IContextMenu contextMenu);

    private bool _isSelected;

    public event SelectStateChanged OnSelectChange;
    public event ContextMenuRequested OnContextMenuRequest;
    
    public void SetSelected(bool isSelected)
    {
        if (_isSelected != isSelected)
        {
            _isSelected = isSelected;
            OnSelectChange?.Invoke(_isSelected);
        }
    }

    public bool TryGetContextMenu(out IContextMenu contextMenu)
    {
        if (OnContextMenuRequest != null && OnContextMenuRequest.Invoke(out contextMenu))
        {
            return true;
        }
        contextMenu = null;
        return false;
    }
}
