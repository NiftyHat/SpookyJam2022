using System;
using Interactions;
using UnityEngine;

namespace UI
{
    public class InputTargetView : MonoBehaviour, ITargetable
    {
        public Vector3 GetInteractionPosition()
        {
            return transform.position;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public bool TryGetGameObject(out GameObject go)
        {
            go = GetGameObject();
            return go != null;
        }

        public event Action<Vector3> OnPositionUpdate;
    }
}