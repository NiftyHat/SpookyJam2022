using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    protected Vector3 _distanceToTarget;

    protected Transform _followTarget;
    protected Vector3 _lookAtPoint;

    [SerializeField] private Transform _transform;
    [SerializeField] private float _distanceToMove = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SetTarget(Transform followTarget)
    {
        _distanceToTarget = transform.position - _followTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distance = _lookAtPoint - _followTarget.position;
        if (distance.magnitude > _distanceToMove)
        {
            Vector3 newCameraTargetPos = _followTarget.position;
            transform.position = Vector3.Lerp(transform.position, newCameraTargetPos, 0.1f);
        }
    }
}
