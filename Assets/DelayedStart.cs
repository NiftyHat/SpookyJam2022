using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedStart : MonoBehaviour
{
[SerializeField] Animator _delayedAnimator;
    // Start is called before the first frame update
    void Start()
    {
_delayedAnimator.enabled=false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.value>0.9f)
{
_delayedAnimator.enabled=true;
}
    }
}
