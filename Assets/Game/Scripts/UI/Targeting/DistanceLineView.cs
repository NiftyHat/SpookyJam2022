using UnityEngine;

namespace UI.Targeting
{
    [ExecuteInEditMode]
    public class DistanceLineView : MonoBehaviour
    {
        [SerializeField] protected LineRenderer _lineRenderer;
        [SerializeField] protected Transform _targetPosition;
    
        public void Update()
        {
            _lineRenderer.SetPosition(0, transform.position);
            if (_targetPosition != null)
            {
                _lineRenderer.SetPosition(1, _targetPosition.position);
            }
        }

        public void SetFrom(Vector3 target)
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, target);
            
        }

        public void SetFrom(Transform transform)
        {
            if (transform != null)
            {
                SetFrom(transform.position);
            }
        }

        public void Clear()
        {
            _lineRenderer.startColor = Color.white;
            _lineRenderer.endColor = Color.white;;
        }

        public void SetColor(Color color)
        {
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
        }
    }
}
