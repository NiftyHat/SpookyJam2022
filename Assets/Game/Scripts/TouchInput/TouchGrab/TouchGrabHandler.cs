using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NiftyFramework.TouchGrab
{
    delegate void GrabStartHandler(Ray touchRay, RaycastHit hitInfo);
    delegate void GrabMoveHandler(Ray touchRay);
    delegate void GrabEndHandler(Ray touchRay);
    public class TouchGrabHandler : MonoBehaviour
    {
        //this is an offset for the grab start point so the object doesn't shift around when being grabbed.
        protected Vector3 _grabAnchor;

        [SerializeField]
        protected Collider _owningPlane;

        public void HandleGrabStart(Ray touchRay, RaycastHit hitInfo)
        {
            _grabAnchor = transform.position - hitInfo.point;
        }

        public void HandleGrabMoved(Vector3 newPosition)
        {
            if (gameObject != null)
            {
                newPosition.z = transform.position.z - _grabAnchor.z;
                transform.position = newPosition + _grabAnchor;
            }
        }

        public void HandleGrabEnd(Ray touchRay)
        {
            _grabAnchor = Vector3.zero;
        }

        public Vector3 GetGrabAnchor() 
        {
            return transform.position + _grabAnchor;
        }


    }
}