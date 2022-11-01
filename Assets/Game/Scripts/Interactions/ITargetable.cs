using System;
using GameStats;
using UnityEngine;

namespace Interactions
{
    public interface ITargetable
    {
        public Vector3 GetInteractionPosition();
        public GameObject GetGameObject();
        public bool TryGetGameObject(out GameObject gameObject);
        public event Action<Vector3> OnPositionUpdate;
    }

    public interface ITargetable<TTarget> : ITargetable
    {
        public TTarget GetInstance();
    }
}