using Data.Interactions;
using Interactions;
using UnityEngine;

namespace UI.Targeting
{
    public class MovementPlaneView : MonoBehaviour, ITargetable
    {
        public InteractionData.TargetType TargetType => InteractionData.TargetType.Floor;

        public Vector3 GetWorldPosition()
        {
            return Vector3.zero;
        }

        public GameObject GetGameObject()
        {
            return null;
        }
    }
}