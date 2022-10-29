using System;
using Data;
using NiftyFramework.Core.Context;
using UnityEngine;

namespace Context
{
    public class TooltipContext : IContext
    {
        public delegate void OnChanged(ITooltip tip);

        private ITooltip _currentTip;

        public event OnChanged OnChange;

        public void Dispose()
        {
        }

        public void Set(ITooltip tip)
        {
            if (tip != _currentTip)
            {
                _currentTip = tip;
                OnChange?.Invoke(_currentTip);
            }
        }

        public void Remove(ITooltip tip)
        {
            if (_currentTip == tip)
            {
                _currentTip = null;
                OnChange?.Invoke(null);
            }
        }

        public void Clear()
        {
            _currentTip = null;
            OnChange?.Invoke(null);
        }

        public void Clear(Func<ITooltip,bool> deltaClear)
        {
            if (deltaClear == null)
            {
                Debug.LogWarning($"{nameof(TooltipContext)} {nameof(Clear)}({nameof(deltaClear)}) {nameof(deltaClear)} cannot be null");
                return;
            }

            if (deltaClear(_currentTip))
            {
                Remove(_currentTip);
            }
        }
    }
}