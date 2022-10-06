using UnityEngine;

namespace NiftyFramework.ScreenInput
{
    public class ScreenInputController : MonoSingleton<ScreenInputController>
    {
        public delegate void InputUpdateHandler(Vector2 position, Vector2 delta, Ray screenPointRay, float time);
        
        private float _inputDownTime = 0; //length of time the mouse/finger is held down for.
        private float _inputStationaryTime = 0; //length of time the mouse/finger is stationary.
        
        private Vector3 _lastMousePosition; //used to track onInputMove/onInput stationary for the mouse

        public Camera mainCamera;

        public event InputUpdateHandler OnInputStart;
        public event InputUpdateHandler OnInputMoved;
        public event InputUpdateHandler OnInputStationary;
        public event InputUpdateHandler OnInputEnd;
        public event InputUpdateHandler OnInputCancel;

        public void Start()
        {
            mainCamera = Camera.main;
        }

        // Update is called once per frame
        public void Update()
        {
            int touchCount = Input.touchCount;
            Ray screenPointRay;
            if (touchCount > 0)
            {
                // Get movement of the finger since last frame
                Touch touchZero = Input.touches[0];
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touchZero.position.x,
                    touchZero.position.y, mainCamera.nearClipPlane));
                
                switch (touchZero.phase)
                {
                    case TouchPhase.Began:
                        _inputStationaryTime = 0;
                        _inputDownTime = 0;
                        
                        if (OnInputStart != null)
                        {
                            screenPointRay = Camera.main.ScreenPointToRay(touchZero.position);
                            OnInputStart(worldPosition, touchZero.deltaPosition, screenPointRay, 0);
                        }

                        break;
                    case TouchPhase.Moved:
                        _inputStationaryTime = 0;
                        _inputDownTime += Time.deltaTime;
                        if (OnInputMoved != null)
                        {
                            screenPointRay = Camera.main.ScreenPointToRay(touchZero.position);
                            OnInputMoved(worldPosition, touchZero.deltaPosition, screenPointRay, _inputDownTime);
                        }

                        break;
                    case TouchPhase.Stationary:
                        _inputStationaryTime += Time.deltaTime;
                        _inputDownTime += Time.deltaTime;
                        if (OnInputStationary != null)
                        {
                            screenPointRay = mainCamera.ScreenPointToRay(touchZero.position);
                            OnInputStationary(worldPosition, touchZero.deltaPosition, screenPointRay, _inputDownTime);
                        }

                        break;
                    case TouchPhase.Ended:
                        _inputStationaryTime = 0;
                        if (OnInputEnd != null)
                        {
                            screenPointRay = mainCamera.ScreenPointToRay(touchZero.position);
                            OnInputEnd(worldPosition, touchZero.deltaPosition, screenPointRay, _inputDownTime);
                        }
                        _inputDownTime = 0;
                        break;
                    case TouchPhase.Canceled:
                        _inputStationaryTime = 0;
                        if (OnInputCancel != null)
                        {
                            screenPointRay = mainCamera.ScreenPointToRay(touchZero.position);
                            OnInputCancel(worldPosition, touchZero.deltaPosition, screenPointRay, _inputDownTime);
                        }
                        _inputDownTime = 0;
                        break;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, mainCamera.nearClipPlane));
                if (Input.GetMouseButtonDown(0))
                {
                    _lastMousePosition = Input.mousePosition;
                    if (OnInputStart != null)
                    {
                        screenPointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                        OnInputStart(worldPosition, Vector2.zero, screenPointRay, _inputDownTime);
                    }
                }
                else if (Input.GetMouseButton(0))
                {
                    Vector2 mouseMoveDelta = _lastMousePosition - Input.mousePosition;
                    _inputDownTime += Time.deltaTime;
                    if (mouseMoveDelta.magnitude > 0.1f)
                    {
                        if (OnInputMoved != null)
                        {
                            screenPointRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                            OnInputMoved(worldPosition, mouseMoveDelta, screenPointRay, _inputDownTime);
                        }
                    }
                    else
                    {
                        if (OnInputStationary != null)
                        {
                            screenPointRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                            OnInputStationary(worldPosition, mouseMoveDelta, screenPointRay, _inputDownTime);
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _inputDownTime += Time.deltaTime;
                    Vector2 mouseMoveDelta = _lastMousePosition - Input.mousePosition;
                    if (OnInputEnd != null)
                    {
                        screenPointRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                        OnInputEnd(worldPosition, mouseMoveDelta, screenPointRay, _inputDownTime);
                    }
                    _inputDownTime = 0;
                }
            }
        }
    }

}