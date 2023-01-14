using System;
using Commands;
using Data;
using Entity;
using GameStats;
using NiftyFramework.Core;
using UI;

namespace Interactions.Commands
{
    public abstract class InteractionCommand : IAsyncCommand
    {
        public class CostProvider : ValueProvider<int>
        {
            public CostProvider(int initialValue) : base(initialValue)
            {
                
            }
        }
        protected readonly IInteraction _interaction;
        protected GameStat _actionPoints;
        
        protected TargetingInfo _targets;
        private bool _showRangeCircle;

        public IInteraction Interaction => _interaction;
        public TargetingInfo Targets => _targets;

        public virtual bool ShowRangeCircle => _interaction.RangeMax > 0;
        public bool ShowRadiusCircle => _interaction.Radius > 0;
        public bool ShowTargetLine => _interaction != null && _interaction.isFloorTarget;

        protected float _distance = 0;
        
        public Range<int> Range { get; }
        public CostProvider APCostProvider { get; }

        public InteractionCommand(IInteraction interaction, TargetingInfo targets, GameStat actionPoints)
        {
            _actionPoints = actionPoints;
            _interaction = interaction;
            _targets = targets;
            if (_targets.Source is PlayerInputHandler player)
            {
                _actionPoints = player.ActionPoints;
            }
            Range = new Range<int>(_interaction.RangeMin, _interaction.RangeMax);
            APCostProvider = new CostProvider(_interaction.CostAP);
        }
        
        public InteractionCommand(IInteraction interaction, TargetingInfo targets)
        {
            _interaction = interaction;
            _targets = targets;
        }

        public virtual bool Validate()
        {
            float distance = _targets.GetDistance();
            if (_interaction == null)
            {
                return false;
            }
            if (_interaction.CostAP > 0 && _actionPoints == null)
            {
                return false;
            }
            if (_interaction.CostAP > 0 && _interaction.CostAP > _actionPoints.Value)
            {
                return false;
            }
            if (!_interaction.IsValidTarget(_targets))
            {
                return false;
            }
            if (_interaction.Radius <= 0)
            {
                if (distance < _interaction.RangeMin)
                {
                    return false;
                }
                if (distance > _interaction.RangeMax)
                {
                    return false;
                }
            }
            return true;
        }


        public bool ValidateRange()
        {
            _distance = _targets.GetDistance();
            return _distance <= _interaction.RangeMax && _distance >= _interaction.RangeMin;
        }

        public int ValidateRadiusTargets()
        {
            if (!ValidateRange())
            {
                return 0;
            }
            TargetingInfo.GetTargetsInRange(_targets.Target, _interaction.Radius, out var targets);
            //targets.Remove(Targets.Target as PointerSelectionHandler);
            return targets.Count;
        }

        public void Update()
        {
            Validate();
        }

        public virtual ITooltip GetTooltip()
        {
            return new TooltipAbilitySimple(_interaction.MenuItem.Icon, _interaction.GetDescription(), _interaction.GetFriendlyName(), _interaction.CostAP);
        }

        public abstract string GetDescription();

        [Obsolete("Use Execute(Complete OnDone) instead")]
        public void Execute()
        {
            throw new NotImplementedException();
        }

        public virtual void Execute(Completed onDone)
        {
            if (!Validate())
            {
                onDone(this, false);
            }
        }

        public void SetTarget(ITargetable targetable)
        {
            _targets = new TargetingInfo(_targets.Source, targetable);
        }

        public bool IsValidTarget(ITargetable floorLocation)
        {
            var targetInfo = new TargetingInfo(_targets.Source, floorLocation);
            return _interaction.IsValidTarget(targetInfo);
        }
    }
}