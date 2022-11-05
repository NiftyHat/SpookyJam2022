using System;
using System.Collections.Generic;
using Commands;
using Entity;
using GameStats;
using Interactions;
using Interactions.Commands;
using TouchInput.UnitControl;
using UI.Targeting;
using UnityEngine;

namespace Data.Interactions
{
    public class MoveInteractionData : InteractionData
    {
        private class Command : InteractionCommand
        {
            private readonly float _oneUnit = 0.1f;
            private float _apCostForOneMovementUnit;
            
            private int _maxRangeFromAP;
            private int _apCostFromDistance;
            
            private UnitMovementHandler _movementHandler;
            private float _apRemainingDistance;
            
            private string _description;
            public Command(IInteraction interaction, TargetingInfo targets, GameStat actionPoints) : base(interaction, targets, actionPoints)
            {
                _apCostForOneMovementUnit = interaction.CostAP / (interaction.RangeMax / _oneUnit);
            }

            public override string GetDescription()
            {
                return _interaction.GetDescription().Replace("{apCost}", APCostProvider.Value.ToString())
                    .Replace("{distance}", _targets.GetDistance().ToString());
            }

            public override void Execute(Completed OnDone)
            {
                if (!Validate())
                {
                    OnDone(this,false);
                    return;
                }
                if (_targets.Source is UnitInputHandler unitInputHandler)
                {
                    var movementHandler = unitInputHandler.gameObject.GetComponent<UnitMovementHandler>();
                    if (movementHandler != null)
                    {
                        movementHandler.MoveTo(_targets.Target.GetInteractionPosition(), endPosition => { OnDone(this, true);;});
                    }
                }
            }
            
            public override bool Validate()
            {
                float distance = _targets.GetDistance();
                int ap = _actionPoints.Value;
                _maxRangeFromAP = (int)(ap / _apCostForOneMovementUnit);
                _apCostFromDistance = (int)(distance / _apCostForOneMovementUnit);
                APCostProvider.Value = _apCostFromDistance;
                if (distance > _maxRangeFromAP)
                {
                    return false;
                }
                return base.Validate();
            }
        }
        
        public override void Init()
        {
            //_movementUnitsPerAp = (_data.CostAP / _data.RangeMax) * _oneUnit;
        }

        public override InteractionCommand GetCommand(TargetingInfo targetingInfo)
        {
            if (targetingInfo.Source is PlayerInputHandler playerInputHandler)
            {
                return new Command(this, targetingInfo, playerInputHandler.ActionPoints);
            }
            return null;
        }
        
        public InteractionCommand GetCommand(PlayerInputHandler player)
        {
            TargetingInfo targetingInfo = new TargetingInfo(player, null);
            return new Command(this, targetingInfo, player.ActionPoints);
        }
    }
}