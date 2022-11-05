using Data.Menu;
using Interactions.Commands;


namespace Interactions
{
    public interface IInteraction
    {
        public int RangeMax { get; }
        public int RangeMin { get; }
        public int CostAP { get; }
        
        public float Radius { get; }
        IMenuItem MenuItem { get; }
        bool isFloorTarget { get; }
        bool isSelfTarget { get; }
        string GetDescription();
        string GetFriendlyName();
        public bool IsValidTarget(TargetingInfo targetingInfo);
        public InteractionCommand GetCommand(TargetingInfo targetingInfo);
    }
}