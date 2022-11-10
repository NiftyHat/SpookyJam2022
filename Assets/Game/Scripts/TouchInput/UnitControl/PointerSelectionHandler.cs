using System;
using Data.Interactions;
using Interactions;
using UI;
using UnityEngine;

namespace TouchInput.UnitControl
{
    public class PointerSelectionHandler : MonoBehaviour, ISelectable
    {
        [SerializeField] private InputTargetView _inputTarget;

        public ITargetable Target => _inputTarget;
        
        public delegate void SelectStateChanged(bool isSelected);

        private bool _isSelected;
        
        private TargetType _targetType;
        public event SelectStateChanged OnSelectChange;
        public event Action<Vector3> OnPositionUpdate;
        public bool IsSelected => _isSelected;

        public void SetSelected(bool isSelected)
        {
            if (_isSelected != isSelected)
            {
                _isSelected = isSelected;
                OnSelectChange?.Invoke(_isSelected);
            }
        }
    }
}
