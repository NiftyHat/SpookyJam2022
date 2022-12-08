using System;
using System.Collections.Generic;
using System.Linq;
using FluentCsv.FluentReader;
using NiftyFramework.ScreenInput;
using UI;
using UI.Targeting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TouchInput.UnitControl
{
    public class PointerSelectInputController : MonoBehaviour
    {
        protected IEnumerable<PointerSelectionHandler> _selected;
        protected HashSet<PointerSelectionHandler> _over;
        protected HashSet<LocationBlockerView> _blockers;
        protected ScreenInputController _inputController;

        protected Vector3 _lastMousePosition;
        public event ScreenInputController.InputUpdateHandler OnPointerMoved;
        public Action<PointerSelectionHandler> OnSelectionChanged;
        public Action<PointerSelectionHandler, RaycastHit> OnSelectionOverChanged;
        public Action<WalkLocationView, RaycastHit> OnSelectGround;
        public Action<WalkLocationView, RaycastHit> OnOverGround;
        
        [SerializeField] protected float _maxDistance = 100f;
        [SerializeField] protected bool _isDebugDraw;

        private int _fingerID = -1;
        private bool _canSelect;

        //private 
        
        void Start()
        {
            _selected = new List<PointerSelectionHandler>();
            _over = new HashSet<PointerSelectionHandler>();
            _blockers = new HashSet<LocationBlockerView>();
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
        
        private static bool TryGetComponentOnCollider<TComponent>(Collider collider, out TComponent component) where TComponent : MonoBehaviour
        {
            if (collider != null)
            {
                component = collider.gameObject.GetComponent<TComponent>();
                if (component == null)
                {
                    component = collider.gameObject.GetComponentInParent<TComponent>();
                }
                return component != null;
            }
            component = null;
            return false;
        }
        
        private static TComponent GetComponentOnCollider<TComponent>(Collider collider) where TComponent : MonoBehaviour
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
            bool isHit = Physics.Raycast(screenPointRay, out hitInfo, _maxDistance, -1, _canSelect ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);
           
            if (isHit)
            {
                if (TryGetComponentOnCollider(hitInfo.collider, out PointerSelectionHandler selectableItem))
                {
                    if (_selected.Any())
                    {
                        foreach (var inputHandler in _selected)
                        {
                            inputHandler.SetSelected(false);
                        }
                    }
                    selectableItem.SetSelected(true);
                    _selected = new[] { selectableItem };
                    OnSelectionChanged?.Invoke(selectableItem);
                }
                else
                {
                    WalkLocationView walkLocationView = GetComponentOnCollider<WalkLocationView>(hitInfo.collider);
                    if (walkLocationView != null)
                    {
                        OnSelectGround?.Invoke(walkLocationView,hitInfo);
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
                PointerSelectionHandler pointerSelectionHandler = GetComponentOnCollider<PointerSelectionHandler>(hitInfo.Value.collider);
                if (pointerSelectionHandler == null)
                {
                    if (GetComponentOnCollider<WalkLocationView>(hitInfo.Value.collider))
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
        
        public struct RayHitComponent<TComponent> where TComponent : MonoBehaviour
        {
            public readonly TComponent Component;
            public readonly RaycastHit RaycastHit;

            public RayHitComponent(RaycastHit raycastHit, TComponent comp)
            {
                Component = comp;
                RaycastHit = raycastHit;
            }
        }

        public struct ParsedResults
        {
            public readonly RayHitComponent<WalkLocationView>? GroundHit;
            public readonly List<RayHitComponent<LocationBlockerView>> BlockerHitList;
            public readonly HashSet<LocationBlockerView> BlockerComps;
            public ParsedResults(RaycastHit[] raycastHits)
            {
                GroundHit = default;
                BlockerHitList = new List<RayHitComponent<LocationBlockerView>>();
                BlockerComps = new HashSet<LocationBlockerView>();
                for (int i = 0; i < raycastHits.Length; i++)
                {
                    RaycastHit hitInfo = raycastHits[i];
                    if (TryGetComponent(hitInfo, out WalkLocationView movementPlaneView))
                    {
                        GroundHit = new RayHitComponent<WalkLocationView>(hitInfo, movementPlaneView);
                    }

                    if (TryGetComponent(hitInfo, out LocationBlockerView locationBlockerView))
                    {
                        RayHitComponent<LocationBlockerView> hitComponent = new RayHitComponent<LocationBlockerView>(hitInfo, locationBlockerView);
                        BlockerHitList.Add(hitComponent);
                        BlockerComps.Add(locationBlockerView);
                    }
                }
            }
        }
        
        public static bool TryGetComponent<TComponent>(RaycastHit hitInfo, out TComponent component) where TComponent : Component
        {
            if (hitInfo.collider == null)
            {
                component = null;
                return false;
            }

            if (TryGetComponent(hitInfo.collider, out component))
            {
                return true;
            }
            return false;
        }
        
        public static bool TryGetComponent<TComponent>(Collider collider, out TComponent component) where TComponent : Component
        {
            component = collider.gameObject.GetComponent<TComponent>();
            if (component == null)
            {
                component = collider.gameObject.GetComponentInParent<TComponent>();
            }
            return component != null;
        }


        private void Update()
        {
            if (IsPointerOverUIElement())
            {
                return;
            }
            _lastMousePosition = Input.mousePosition;
            Ray screenPointRay = _inputController.mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hitResults = new RaycastHit[50];
            if (Physics.RaycastNonAlloc(screenPointRay, hitResults) > 0)
            {
                ParsedResults parsedResults = new ParsedResults(hitResults);
                if (parsedResults.GroundHit.HasValue)
                {
                    var result = parsedResults.GroundHit.Value;
                    OnOverGround?.Invoke(result.Component, result.RaycastHit);
                }

                if (!_canSelect)
                {
                    foreach (var item in _blockers)
                    {
                        if (parsedResults.BlockerComps != null)
                        {
                            if (!parsedResults.BlockerComps.Contains(item))
                            {
                                item.SetBlocking(false);
                            }
                        }
                    }

                    if (parsedResults.BlockerComps != null)
                    {
                        foreach (var item in parsedResults.BlockerComps)
                        {
                            if (!_blockers.Contains(item))
                            {
                                item.SetBlocking(true);
                            }
                        }
                    }
                }

                _blockers = parsedResults.BlockerComps;

                if (parsedResults.BlockerHitList.Count > 0)
                {
                    foreach (var item in parsedResults.BlockerHitList)
                    {
                        if (!_blockers.Contains(item.Component))
                        {
                            _blockers.Add(item.Component);
                            item.Component.SetBlocking(true);
                        }
                    }
                }
            }
            if (Physics.Raycast(screenPointRay, out RaycastHit hitInfo, _maxDistance))
            {
                var groundPlane = GetComponentOnCollider<WalkLocationView>(hitInfo.collider);
                if (groundPlane != null)
                {
                    OnOverGround?.Invoke(groundPlane, hitInfo);
                }
                
                if (TryGetComponentOnCollider(hitInfo.collider, out PointerSelectionHandler selectableItem))
                {
                    if (!_over.Contains(selectableItem))
                    {
                        _over.Add(selectableItem);
                        selectableItem.SetOver(true);
                        OnSelectionOverChanged?.Invoke(selectableItem, hitInfo);
                    }
                }
                else
                {
                    if (_over != null && _over.Count > 0)
                    {
                        foreach (var item in _over)
                        {
                            item.SetOver(false);
                        }
                        OnSelectionOverChanged?.Invoke(null, hitInfo);
                        _over.Clear();
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
                _selected = new List<PointerSelectionHandler>();
                OnSelectionChanged?.Invoke(null);
            }
        }
        
        //Returns 'true' if we touched or hovering on Unity UI element.
        public bool IsPointerOverUIElement()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(_fingerID))    // is the touch on the GUI
            {
                return true;
            }

            return false;
        }
        
        public static bool GetTargetsInRadius<TTarget>(Vector3 origin, float radius, out List<TTarget> targets, Func<TTarget, bool> filter = null) where TTarget : MonoBehaviour
        {
            targets = new List<TTarget>();
            Collider[] results = new Collider[50];
            var size = Physics.OverlapSphereNonAlloc(origin, radius, results);
            for (int i = 0; i < size; i++)
            {
                Collider collider = results[i];
                var unit = GetComponentOnCollider<TTarget>(collider);
                if (unit != null)
                {
                    if (filter == null)
                    {
                        targets.Add(unit);
                    }
                    else if (filter(unit))
                    {
                        targets.Add(unit);
                    }
                };
            }
            return targets.Count > 0;
        }

        public void SetCanSelect(bool canSelect)
        {
            _canSelect = canSelect;
            if (_canSelect && _blockers.Count > 0)
            {
                foreach (var item in _blockers)
                {
                    item.SetBlocking(false);
                }
                _blockers.Clear();
            }
        }
    }
}