using UnityEditor;
using UnityEngine;

public class LabledTransform : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Handles.Label(transform.position,name);
    }
}