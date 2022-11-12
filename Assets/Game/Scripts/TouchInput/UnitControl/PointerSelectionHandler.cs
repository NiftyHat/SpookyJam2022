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
        public delegate void OverStateChanged(bool isSelected);

        private bool _isSelected;
        
        private TargetType _targetType;
        public event SelectStateChanged OnSelectChange;
        public event OverStateChanged OnOverStateChange;
        public bool IsSelected => _isSelected;

        public bool IsOver => _isOver;
        private bool _isOver;
        
        public event Action<Vector3> OnPositionUpdate;

        public void SetSelected(bool isSelected)
        {
            if (_isSelected != isSelected)
            {
                _isSelected = isSelected;
                OnSelectChange?.Invoke(_isSelected);
            }
        }

        public void SetOver(bool isOver)
        {
            if (_isOver != isOver)
            {
                _isOver = isOver;
                OnOverStateChange?.Invoke(isOver);
            }
        }
    }
}
