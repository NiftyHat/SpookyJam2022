using Commands;
using Context;
using Entity;
using GameStats;
using Interactions;
using Interactions.Commands;
using Newtonsoft.Json.Serialization;
using NiftyFramework.Core.Context;
using TouchInput.UnitControl;
using UnityEngine;

namespace Data.Interactions
{
    public class MoveInteractionData : InteractionData
    {
        public class Command : InteractionCommand
        {
            private readonly float _oneUnit = 2f;
            private float _apCostForOneMovementUnit;
            
            private int _maxRangeFromAP;
            private int _apCostFromDistance;
            
            private UnitMovementHandler _movementHandler;
            private float _apRemainingDistance;

            private readonly LayerMask _movementBlocker = LayerMask.GetMask("Movement Blocker");
            private Vector3 _targetLocation;
            public Vector3 TargetLocation => _targetLocation;

            public override bool ShowRangeCircle => false;

            private string _description;
            private bool _showRangeCircle1;

            public Command(IInteraction interaction, TargetingInfo targets, GameStat actionPoints) : base(interaction, targets, actionPoints)
            {
                _apCostForOneMovementUnit = _oneUnit;
            }

            public override string GetDescription()
            {
                return _interaction.GetDescription().Replace("{apCost}", APCostProvider.Value.ToString())
                    .Replace("{distance}", _targets.GetDistance().ToString());
            }

            public Vector3 GetMoveLocation()
            {
                Vector3 castStart= _targets.Source.GetInteractionPosition();
                if (_targets.Target != null)
                {
                    
                    Vector3 castEnd = _targets.Target.GetInteractionPosition();
#if UNITY_EDITOR
                    Debug.DrawLine(castStart, castEnd, Color.yellow, 0.1f);
#endif
                    if (Physics.Linecast(castStart, castEnd, out RaycastHit raycastHit, _movementBlocker))
                    {
                        Vector3 castLocation = new Vector3(raycastHit.point.x, castEnd.y, raycastHit.point.z);
                        
#if UNITY_EDITOR
                        Debug.DrawLine(castStart, castLocation, Color.red, 0.1f);
#endif
                        return castLocation;
                    }
                    return castEnd;
                }
                
                return castStart;
            }

            public override void Execute(Completed OnDone)
            {
                if (!Validate())
                {
                    OnDone(this,false);
                    return;
                }
                
                if (_targets.Source.TryGetGameObject(out var go))
                {
                    var movementHandler = go.GetComponent<UnitMovementHandler>();
                    if (movementHandler != null)
                    {
                        _actionPoints.Subtract(APCostProvider.Value);
                        movementHandler.MoveTo(_targetLocation, endPosition =>
                        {
                            if (_targets.Source is PlayerInputHandler player)
                            {
                                Collider[] overlappingItems = new Collider[10];
                                var size = Physics.OverlapSphereNonAlloc(endPosition, 1f, overlappingItems);
                                for (int i = 0; i < size; i++)
                                {
                                    Collider collider = overlappingItems[i];
                                    if (PointerSelectInputController.TryGetComponent(collider, out TransitionZoneView comp))
                                    {
                                        ContextService.Get<GameStateContext>(gameState =>
                                        {
                                            gameState.ChangeLocation(player, comp.LinkedLocation, comp.ZoneLocation);
                                        });
                                        OnDone(this,true);
                                        return;
                                    }
                                }
                                OnDone(this, true);
                            }
                            else
                            {
                                OnDone(this, true);
                            }
                        });
                    }
                }
            }
            
            
            public override bool Validate()
            {
                _targetLocation = GetMoveLocation();
                float distance = Vector3.Distance(_targets.Source.GetInteractionPosition(), _targetLocation);
                var abilityRange = Range;
                int ap = _actionPoints.Value;
                _maxRangeFromAP = (int)(ap * _apCostForOneMovementUnit);
                _apCostFromDistance = (int)(distance / _apCostForOneMovementUnit);
                APCostProvider.Value = _apCostFromDistance;
                int maxRange = Mathf.Min(_interaction.RangeMax, _maxRangeFromAP);
                abilityRange.Max = maxRange;
                if (distance > _maxRangeFromAP)
                {
                    return false;
                }

                
                return base.Validate();
            }
            
            public override bool ValidateRange()
            {
                return Validate();
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