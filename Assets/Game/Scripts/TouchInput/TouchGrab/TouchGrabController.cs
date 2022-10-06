using System;
using System.Collections;
using System.Collections.Generic;
using NiftyFramework.ScreenInput;
using UnityEngine;

namespace NiftyFramework.TouchGrab
{
    public class TouchGrabController : MonoBehaviour
    {

        protected ScreenInputController inputController;

        public bool isDrawDebug;

        public float maxDistance = 100f;

        protected float _hitDistance = 0f;

        [SerializeField]
        protected TouchGrabHandlerRuntimeSet _heldItemList;

        void Start()
        {

            inputController = ScreenInputController.instance;
            inputController.OnInputStart += OnInputStart;
            inputController.OnInputMoved += OnInputUpdate;
            //inputController.OnInputStationary += OnInputUpdate;
            inputController.OnInputEnd += OnInputEnd;
            if (Application.isEditor == false)
            {
                //isDrawDebug = false;
            }
        }

        private TouchGrabHandler GetTouchGrabHandler(Collider collider)
        {
            if (collider != null)
            {
                TouchGrabHandler grabHandler = collider.gameObject.GetComponent<TouchGrabHandler>();
                if (grabHandler == null)
                {
                    grabHandler = collider.gameObject.GetComponentInParent<TouchGrabHandler>();
                }
                return grabHandler;
            }
            return null;
        }

        private void OnInputStart(Vector2 position, Vector2 delta, Ray screenPointRay, float time)
        {
            _heldItemList.Clear();
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(screenPointRay, out hitInfo, maxDistance);
            TouchGrabHandler touchGrabHandler = GetTouchGrabHandler(hitInfo.collider);

            if (touchGrabHandler != null)
            {
                _heldItemList.Add(touchGrabHandler);
                touchGrabHandler.HandleGrabStart(screenPointRay, hitInfo);
                _hitDistance = Vector3.Distance(screenPointRay.origin, hitInfo.point);
            }
            DrawDebug(screenPointRay, maxDistance, hitInfo);
        }

        private void OnInputUpdate(Vector2 position, Vector2 delta, Ray screenPointRay, float time)
        {
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(screenPointRay, out hitInfo, maxDistance);
            
            Vector3 newPostion = screenPointRay.origin + (screenPointRay.direction * _hitDistance);
            _heldItemList.HandleMove(hitInfo.point);
            DrawDebug(screenPointRay, maxDistance, hitInfo);
        }

        private void OnInputEnd(Vector2 position, Vector2 delta, Ray screenPointRay, float time)
        {
            _heldItemList.HandleEnd(screenPointRay);

            DrawDebug(screenPointRay, maxDistance, null);
        }


        private void DrawDebug(Ray touchRay, float maxDistance, RaycastHit? hitInfo = null)
        {
            Color rayColor = Color.white;

            if (_heldItemList.Count() > 0)
            {
                _heldItemList.With((TouchGrabHandler handler) =>
                {
                    Debug.DrawLine(touchRay.origin, handler.GetGrabAnchor(), Color.green, 0.1f);
                });
            }
            else
            {
                Vector3 rayEnd;
                if (hitInfo != null && hitInfo.Value.collider)
                {
                    TouchGrabHandler touchGrabHandler = GetTouchGrabHandler(hitInfo.Value.collider);
                    if (touchGrabHandler == null)
                    {
                        rayColor = Color.red;
                    }
                    rayEnd = hitInfo.Value.point;
                }
                else
                {
                    rayEnd = touchRay.origin + (touchRay.direction * maxDistance);
                }
                Debug.DrawLine(touchRay.origin, rayEnd, rayColor, 0.1f);
            }

        }

    }
}