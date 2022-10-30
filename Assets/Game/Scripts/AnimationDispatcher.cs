using System;
using UnityEngine;

namespace Interactions
{
    [ExecuteInEditMode]
    public class AnimationDispatcher : MonoBehaviour
    {
        public event Action<string> OnMessage;
        
        public void DispatchMessage(string message)
        {
            if (Application.isEditor)
            {
                Debug.Log($"{name} {nameof(AnimationDispatcher)}.{nameof(DispatchMessage)}({message})");
            }
            OnMessage?.Invoke(message);
        }
    }
}