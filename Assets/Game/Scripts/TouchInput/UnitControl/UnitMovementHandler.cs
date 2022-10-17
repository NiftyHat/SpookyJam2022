using System;
using UnityEngine;

namespace TouchInput.UnitControl
{
    public class UnitMovementHandler : MonoBehaviour
    {
        [SerializeField] private float _velocity;
        [SerializeField] private AnimationCurve _accelerationCurve;

        public bool IsMoving => _moveLocation.HasValue;
        
        public Vector3? MoveLocation => _moveLocation;
        private Vector3? _moveLocation;

        public Vector3 MoveDirection => _moveDirection;
        private Vector3 _moveDirection;

        private Vector3 _lastVelocity;

        public event Action<Vector3> OnMoveComplete;

        public void Update()
        {
            if (_moveLocation.HasValue)
            {
                Vector3 movementDelta = _moveDirection * _velocity * Time.deltaTime;
                if (Vector3.Distance(transform.position, _moveLocation.Value) > movementDelta.magnitude)
                {
                    transform.position += movementDelta;
                }
                else
                {
                    transform.position = _moveLocation.Value;
                    
                    EndMovement();
                }
            }
        }

        public void EndMovement()
        {
            Vector3 endLocation = _moveLocation.Value;
            _moveLocation = null;
            _moveDirection = Vector3.one;
            OnMoveComplete?.Invoke(endLocation);
        }
        
        public void MoveTo(Vector3 raycastPoint, Action<Vector3> onComplete = null)
        {
            var position = transform.position;
            Vector3 moveLocation = new Vector3(raycastPoint.x, position.y, raycastPoint.z);
            _moveLocation = moveLocation;
            _moveDirection = (_moveLocation.Value - position).normalized;
            if (onComplete != null)
            {
                OnMoveComplete += onComplete;
            }
        }
    }
}