using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    protected Vector3 _relativeVectorFromTarget;
    
    protected Vector3 _lookAtPoint;

    [SerializeField] private Transform _followTarget;
    [SerializeField] private float _distanceToMove = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (_followTarget != null)
        {
            //return 
        }
    }

    void SetTarget(Transform followTarget)
    {
        _followTarget = followTarget;
        _relativeVectorFromTarget = transform.position - _followTarget.position;
        _lookAtPoint = transform.position + _followTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_lookAtPoint == Vector3.zero)
        {
            SetTarget(_followTarget);
        }
        Vector3 distance = _lookAtPoint - _followTarget.position;
        if (distance.magnitude > _distanceToMove)
        {
            Vector3 newCameraTargetPos = _followTarget.position + _relativeVectorFromTarget;
            transform.position = Vector3.Lerp(transform.position, newCameraTargetPos, 0.005f);
            _lookAtPoint = transform.position - _relativeVectorFromTarget;
        }
    }
}
