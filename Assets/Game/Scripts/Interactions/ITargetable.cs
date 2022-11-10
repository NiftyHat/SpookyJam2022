using System;
using UnityEngine;

namespace Interactions
{
    public interface ITargetable
    {
        public Vector3 GetInteractionPosition();
        public GameObject GetGameObject();
        public bool TryGetGameObject(out GameObject gameObject);
    }

    public interface ITargetable<TTarget> : ITargetable
    {
        public TTarget GetInstance();
    }
}