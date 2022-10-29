using UnityEngine;

namespace Interactions
{
    public interface ITargetable
    {
        public Vector3 GetWorldPosition();
    }

    public interface ITargetable<TTarget> : ITargetable
    {
        public TTarget GetTarget();
    }
}