using System;
using Interactions;
using UnityEngine;

namespace UI.Targeting
{
    public class MovementPlaneView : MonoBehaviour, ITargetable
    {
        public Vector3 GetInteractionPosition()
        {
            return Vector3.zero;
        }

        public GameObject GetGameObject()
        {
            return null;
        }

        public bool TryGetGameObject(out GameObject go)
        {
            go = null;
            return false;
        }

        public event Action<Vector3> OnPositionUpdate;
    }
}