using System;
using System.Collections.Generic;
using System.Linq;
using NiftyFramework.ScreenInput;
using UI.Targeting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TouchInput.UnitControl
{
    public class UnitInputController : MonoBehaviour
    {
        protected IEnumerable<UnitInputHandler> _selected;
        protected ScreenInputController _inputController;

        protected Vector3 _lastMousePosition;
        public event ScreenInputController.InputUpdateHandler OnPointerMoved;
        public Action<UnitInputHandler> OnUnitSelected;
        public Action<MovementPlaneView, RaycastHit> OnGroundSelected;
        
        [SerializeField] protected float _maxDistance = 100f;
        [SerializeField] protected bool _isDebugDraw;
        [SerializeField] protected LayerMask _layer;
        
         private int _fingerID = -1;
        
        void Start()
        {
            _selected = new List<UnitInputHandler>();
            _inputController = ScreenInputController.instance;
            _inputController.OnInputStart += HandleInputStart;
            _inputController.OnInputMoved += HandleInputUpdate;
            _inputController.OnInputStationary +=  HandleInputStationary;
            _inputController.OnInputEnd += HandleInputEnd;
            if (Application.isEditor == false)
            {
                //isDrawDebug = false;
            }
            #if !UNITY_EDITOR
                 _fingerID = 0; 
            #endif
        }

        private void HandleInputStationary(Vector2 position, Vector2 delta, Ray screenPointRay, float time)
        {
        }

        private void HandleInputEnd(Vector2 position, Vector2 delta, Ray screenPointRay, float time)
        {
            //throw new System.NotImplementedException();
        }
        
        private TComponent GetComponentOnCollider<TComponent>(Collider collider) where TComponent : MonoBehaviour
        {
            if (collider != null)
            {
                TComponent component = collider.gameObject.GetComponent<TComponent>();
                if (component == null)
                {
                    component = collider.gameObject.GetComponentInParent<TComponent>();
                }
                return component;
            }
            return null;
        }
        
        /*
        private UnitInputHandler GetUnitInputHandler(Collider collider)
        {
            if (collider != null)
            {
                UnitInputHandler unitInputHandler = collider.gameObject.GetComponent<UnitInputHandler>();
                if (unitInputHandler == null)
                {
                    unitInputHandler = collider.gameObject.GetComponentInParent<UnitInputHandler>();
                }
                return unitInputHandler;
            }
            return null;
        }*/

        private void HandleInputStart(Vector2 position, Vector2 delta, Ray screenPointRay, float time)
        {
            if (IsPointerOverUIElement())
            {
                return;
            }
            //_heldItemList.Clear();
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(screenPointRay, out hitInfo, _maxDistance);
            UnitInputHandler unitInputHandler = GetComponentOnCollider<UnitInputHandler>(hitInfo.collider);

            if (isHit)
            {
                if (unitInputHandler != null)
                {
                    if (_selected.Any())
                    {
                        foreach (var inputHandler in _selected)
                        {
                            inputHandler.SetSelected(false);
                        }
                    }
                    unitInputHandler.SetSelected(true);
                    _selected = new[] { unitInputHandler };
                    OnUnitSelected?.Invoke(unitInputHandler);
                }
                else
                {
                    MovementPlaneView movementPlaneView = GetComponentOnCollider<MovementPlaneView>(hitInfo.collider);
                    if (movementPlaneView != null)
                    {
                        OnGroundSelected?.Invoke(movementPlaneView,hitInfo);
                    }
                }
            }
            
        }
        
        private void HandleInputUpdate(Vector2 position, Vector2 delta, Ray screenPointRay, float time)
        {
            //throw new System.NotImplementedException();
        }
        
        private void DrawDebug(Ray touchRay, float maxDistance, RaycastHit? hitInfo = null)
        {
            if (IsPointerOverUIElement())
            {
                return;
            }
            Color rayColor = Color.white;
            Vector3 rayEnd;
            if (hitInfo != null && hitInfo.Value.collider)
            {
                UnitInputHandler unitInputHandler = GetComponentOnCollider<UnitInputHandler>(hitInfo.Value.collider);
                if (unitInputHandler == null)
                {
                    if (GetComponentOnCollider<MovementPlaneView>(hitInfo.Value.collider))
                    {
                        rayColor = Color.yellow;
                    }
                    else
                    {
                        rayColor = Color.red;
                    }
                }
                else
                {
                    rayColor = Color.green;
                }
                rayEnd = hitInfo.Value.point;
            }
            else
            {
                rayEnd = touchRay.origin + (touchRay.direction * maxDistance);
            }
            Debug.DrawLine(touchRay.origin, rayEnd, rayColor, 0.1f);

        }
        
        private void Update()
        {
            if (IsPointerOverUIElement())
            {
                return;
            }
            _lastMousePosition = Input.mousePosition;
            var screenPointRay = _inputController.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(screenPointRay, out var hitInfo, _maxDistance))
            {
                if (_selected.Any())
                {
                    var first = _selected.FirstOrDefault();
                    if (first != null)
                    {
                        if (first.TryGetInteraction(out var interaction))
                        {
                            interaction.PreviewInput(hitInfo);
                        }
                    }
                }
                
                if (_isDebugDraw)
                {
                    DrawDebug(screenPointRay, _maxDistance, hitInfo);
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                if (_selected.Any())
                {
                    foreach (var inputHandler in _selected)
                    {
                        inputHandler.SetSelected(false);
                    }
                }
                _selected = new List<UnitInputHandler>();
                OnUnitSelected?.Invoke(null);
            }
        }
        
        //Returns 'true' if we touched or hovering on Unity UI element.
        public bool IsPointerOverUIElement()
        {
            if (EventSystem.current.IsPointerOverGameObject(_fingerID))    // is the touch on the GUI
            {
                /*
                PointerEventData pointerData = new PointerEventData(EventSystem.current) {
                    pointerId = -1,
                };
                
                
                pointerData.position = Input.mousePosition;

                var list  = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, list);*/
                
                // GUI Action
                return true;
            }

            return false;
        }
    }
}