using System;
using Data.Interactions;
using Data.Menu;
using UnityEngine;

namespace Interactions
{
    public interface IInteraction
    {
        public int Range { get; }
        public int ApCost { get; }
        
        public ITargetable Source { get; }
        public ITargetable Target { get; }
        public Vector3? TargetPosition { get; }

        public void SetParent(ITargetable parent);

        public bool IsState(InteractionData.State testState);

        public event Action OnComplete;

        public event Action<int> OnApCostChange;

        IMenuItem MenuItem { get; }

        bool PreviewInput(TargetingInfo targetingInfo);
        bool PreviewInput(RaycastHit hitInfo);

        bool ConfirmInput(TargetingInfo targetingInfo);
        bool ConfirmInput(RaycastHit hitInfo);

        bool ValidateRange(float distance);

        float GetMaxRange();
        string GetDescription();
        void ClearSelect();
    }
}