using UnityEngine;

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

        public float GetDistance()
        {
            if (_source != null && _target != null)
            {
                Vector3 sourcePos = _source.GetInteractionPosition();
                Vector3 targetPos = _target.GetInteractionPosition();
                return Vector3.Distance(sourcePos, targetPos);
            }
            return 0;
        }

        public Vector3 DirectionToTarget()
        {
            if (_target != null)
            {
                return (_source.GetInteractionPosition() - _target.GetInteractionPosition()).normalized;
            }
            return Vector3.zero;
        }
    }
    
    public interface ITargetingInfo <TSource, TTarget> where TSource : ITargetable where TTarget : ITargetable
    {
        public TSource Source { get; }
        public TTarget Target { get; }
    }
}