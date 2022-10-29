using GameStats;
using Interactions;
using TouchInput.UnitControl;
using UnityEngine;

namespace Data.Interactions
{
    public class MoveInteractionData : InteractionData
    {
        [SerializeField] private float apCostPerUnit = 1.0f;
        
        private UnitMovementHandler _movementHandler;
        private GameStat _actionPoints;
        private float _distance;

        public override int ApCost
        {
            get
            {
                if (_source != null && _targetPosition != Vector3.zero)
                {
                    return GetMoveAPCost(Vector3.Distance(_source.GetWorldPosition(), _targetPosition));
                };
                return _apCost;
            }
        }

        public override void SetParent(ITargetable parent)
        {
            if (parent is UnitInputHandler unitInputHandler)
            {
                _movementHandler = unitInputHandler.gameObject.GetComponent<UnitMovementHandler>();
            }
            base.SetParent(parent);
        }

        public override bool ConfirmInput(RaycastHit hitInfo)
        {
            if (base.ConfirmInput(hitInfo))
            {
                _parentStats.ActionPoints.Subtract(ApCost);
                _movementHandler.MoveTo(hitInfo.point, HandleMoveComplete);
                return true;
            }
            return false;
        }

        private void HandleMoveComplete(Vector3 location)
        {
            InternalComplete();
        }

        public override bool ValidateRange(float distance)
        {
            return CanAfford();
        }

        public override float GetMaxRange()
        {
            if (_parentStats != null)
            {
                return _parentStats.ActionPoints.Value * apCostPerUnit;
            }

            return 0;
        }

        public int GetMoveAPCost(float distance)
        {
            float moveCost = (_range / _apCost) * apCostPerUnit * distance;
            return Mathf.FloorToInt(moveCost);
        }
    }
}