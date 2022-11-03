using Data.Menu;
using Interactions.Commands;


namespace Interactions
{
    public interface IInteraction
    {
        public int RangeMax { get; }
        public int RangeMin { get; }
        public int CostAP { get; }
        IMenuItem MenuItem { get; }
        bool isFloorTarget { get; }
        string GetDescription();
        string GetFriendlyName();
        public bool IsValidTarget(ITargetable target);
        public InteractionCommand GetCommand(TargetingInfo targetingInfo);
    }
}