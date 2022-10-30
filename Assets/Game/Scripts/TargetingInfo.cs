namespace Interactions
{

    public struct TargetingInfo : ITargetingInfo<ITargetable, ITargetable>
    {
        private readonly ITargetable _source;
        private readonly ITargetable _target;

        public ITargetable Source => _source;
        public ITargetable Target => _target;

        public TargetingInfo(ITargetable source, ITargetable target)
        {
            _source = source;
            _target = target;
        }
    }
    
    public interface ITargetingInfo <TSource, TTarget> where TSource : ITargetable where TTarget : ITargetable
    {
        public TSource Source { get; }
        public TTarget Target { get; }
    }
}