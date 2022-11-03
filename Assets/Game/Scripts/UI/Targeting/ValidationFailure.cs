using Data.Interactions;
using Interactions;

namespace UI.Targeting
{
    public interface IValidationFailure
    {
        public string Copy { get; }
    }

    public struct InvalidTarget : IValidationFailure
    {
        public string Copy { get; }
        public InvalidTarget(string title, ITargetable target, TargetType type)
        {
            if (target.TryGetGameObject(out var gameObject))
            {
                Copy = $"{title}: {gameObject.name} not valid type {type.ToString()}";
            }
            Copy = $"{target.GetInteractionPosition()} not valid type {type.ToString()}";
        }
    }
        
    public struct OutOfRange : IValidationFailure
    {
        public string Copy { get; }
        
        public OutOfRange(string title, float distance, float maxRange)
        {
            Copy = $"{title}: {distance} greater than max range of {maxRange}";
        }
    }
    
    public struct TooClose : IValidationFailure
    {
        public string Copy { get; }
        
        public TooClose(string title, float distance, float minRange)
        {
            Copy = $"{title}: {distance} must be further than {minRange}";
        }
    }
    
    public struct RequiresActionPoints : IValidationFailure
    {
        public string Copy { get; }

        public RequiresActionPoints(string title)
        {
            Copy = $"{title} Requires a StatBlock with ActionPoints";
        }
    }
}