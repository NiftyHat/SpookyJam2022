using System.Collections.Generic;
using System.Linq;
using NiftyFramework.ScreenInput;
using UI.ContextMenu;
using UnityEngine;

namespace TouchInput.UnitControl
{
    public class UnitInputController : MonoBehaviour
    {
        protected IContextMenu _contextMenu;
        protected IEnumerable<UnitInputHandler> _selected;
        protected ScreenInputController inputController;
        
        [SerializeField] protected float _maxDistance = 100f;
        
        void Start()
        {
            _selected = new List<UnitInputHandler>();
            inputController = ScreenInputController.instance;
            inputController.OnInputStart += HandleInputStart;
            inputController.OnInputMoved += HandleInputUpdate;
            inputController.OnInputStationary +=  HandleInputStationary;
            inputController.OnInputEnd += HandleInputEnd;
            if (Application.isEditor == false)
            {
                //isDrawDebug = false;
            }
        }

        private void HandleInputStationary(Vector2 position, Vector2 delta, Ray screenPointRay, float time)
        {
            if (_selected != null && time > 1.0f)
            {
                var first = _selected.First();
                if (first != null && first.TryGetContextMenu(out var contextMenu))
                {
                    _contextMenu = contextMenu;
                }
            }
        }

        private void HandleInputEnd(Vector2 position, Vector2 delta, Ray screenPointRay, float time)
        {
            //throw new System.NotImplementedException();
        }
        
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
        }

        private void HandleInputStart(Vector2 position, Vector2 delta, Ray screenPointRay, float time)
        {
            //_heldItemList.Clear();
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(screenPointRay, out hitInfo, _maxDistance);
            UnitInputHandler unitInputHandler = GetUnitInputHandler(hitInfo.collider);

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
            }
            DrawDebug(screenPointRay, _maxDistance, hitInfo);
        }
        
        private void HandleInputUpdate(Vector2 position, Vector2 delta, Ray screenPointRay, float time)
        {
            //throw new System.NotImplementedException();
        }
        
        private void DrawDebug(Ray touchRay, float maxDistance, RaycastHit? hitInfo = null)
        {
            Color rayColor = Color.white;

            if (_selected != null && _selected.Any())
            {
                foreach (var handler in _selected)
                {
                    Debug.DrawLine(touchRay.origin, handler.transform.position, Color.green, 0.1f);
                }
            }
            else
            {
                Vector3 rayEnd;
                if (hitInfo != null && hitInfo.Value.collider)
                {
                    UnitInputHandler unitInputHandler = GetUnitInputHandler(hitInfo.Value.collider);
                    if (unitInputHandler == null)
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