using Commands;
using Context;
using Data.Location;
using Data.Menu;
using GameStats;
using Interactions;
using Interactions.Commands;
using NiftyFramework.Core.Context;
using UI;

namespace Data.Interactions
{
    public class FastTravelInteraction : IInteraction
    {
        public class Command : InteractionCommand
        {
            private LocationData _location;
            
            public Command(IInteraction interaction, TargetingInfo targets, GameStat actionPoints, LocationData location) : base(interaction, targets, actionPoints)
            {
                _location = location;
            }

            public Command(IInteraction interaction, TargetingInfo targets) : base(interaction, targets)
            {
            }

            public override void Execute(Completed onDone)
            {
                base.Execute(onDone);
            }

            public override string GetDescription()
            {
                return _interaction.GetDescription().Replace("{location}", _location.FriendlyName);
            }
        }
        
        private int _rangeMax;
        private int _rangeMin;
        private int _costAP;
        private float _radius;
        private IMenuItem _menuItem;
        
        public int RangeMax => 0;

        public int RangeMin => 0;

        public int CostAP => _costAP;

        public float Radius => 0;

        public IMenuItem MenuItem => _menuItem;

        public bool isFloorTarget => false;

        public bool isSelfTarget => false;

        public string GetDescription()
        {
            return "Fast Travel to {destination}";
        }

        public string GetFriendlyName()
        {
            return "Fast Travel";
        }

        public bool IsValidTarget(TargetingInfo targetingInfo)
        {
            return true;
        }

        public InteractionCommand GetCommand(TargetingInfo targetingInfo)
        {
            throw new System.NotImplementedException();
        }

        public ITooltip GetTooltip()
        {
            return new TooltipAbilitySimple(MenuItem.Icon, GetDescription(), GetFriendlyName(), _costAP);
        }
    }
}