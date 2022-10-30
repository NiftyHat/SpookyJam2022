using UnityEngine;

namespace Interactions
{
    public interface ITargetable
    {
        public Vector3 GetWorldPosition();
        public GameObject GetGameObject();
    }

    public interface ITargetable<TTarget> : ITargetable
    {
        public TTarget GetInstance();
    }
}