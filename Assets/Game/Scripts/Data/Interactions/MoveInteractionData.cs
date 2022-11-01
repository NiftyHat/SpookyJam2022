using System.Collections.Generic;
using Entity;
using Interactions;
using TouchInput.UnitControl;
using UI.Targeting;
using UnityEngine;

namespace Data.Interactions
{
    public class MoveInteractionData : InteractionData
    {
        private readonly float _oneUnit = 0.1f;
        private float _apCostForOneMovementUnit = 0;
        public override int RangeMax => _maxRangeFromAP;

        public override int CostAP => _apCostFromDistance;

        private UnitMovementHandler _movementHandler;
        private float _apRemainingDistance;
        private int _maxRangeFromAP;
        private int _apCostFromDistance;

        public override void Init()
        {
            _apCostForOneMovementUnit = _data.CostAP / (_data.RangeMax / _oneUnit);
            //_movementUnitsPerAp = (_data.CostAP / _data.RangeMax) * _oneUnit;
        }
        
        
        public override string GetDescription()
        {
            return base.GetDescription().Replace("{costPerUnit}", _apCostFromDistance.ToString());
        }

        public override bool Validate(TargetingInfo targetingInfo, ref IList<IValidationFailure> invalidators)
        {
            if (targetingInfo.Source is PlayerInputHandler player)
            {
                _actionPoints = player.ActionPoints;
            }
            float distance = targetingInfo.GetDistance();
            int ap = _actionPoints.Value;
            _maxRangeFromAP = (int)(ap / _apCostForOneMovementUnit);
            _apCostFromDistance = (int)(distance * _apCostForOneMovementUnit);
            return base.Validate(targetingInfo, ref invalidators);
        }
        
        public override bool Confirm(TargetingInfo targetInfo)
        {
            if (base.Confirm(targetInfo))
            {
                if (targetInfo.Source is UnitInputHandler unitInputHandler)
                {
                    _movementHandler = unitInputHandler.gameObject.GetComponent<UnitMovementHandler>();
                    if (_movementHandler != null)
                    {
                        _movementHandler.MoveTo(targetInfo.Target.GetInteractionPosition(), HandleMoveComplete);
                        return true;
                    }
                }
            }
            return false;
        }

        private void HandleMoveComplete(Vector3 obj)
        {
            InternalComplete();
        }
    }
}