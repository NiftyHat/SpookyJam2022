#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class LabledTransform : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        Handles.Label(transform.position,name);
        #endif
    }
}