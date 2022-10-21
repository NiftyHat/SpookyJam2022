using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class ToggleWidget<T> : MonoBehaviour
{

    [SerializeField]
    protected T _data;
    public T Data => _data;

    public bool Value => toggle.isOn;
    [SerializeField]
    protected Toggle toggle;
    [SerializeField]
    protected Text label;

    public UnityEvent onSetTrue;

    public abstract void Initialize(T data, bool value);

    public void SetValue(bool value)
    {
        toggle.SetIsOnWithoutNotify(value);
        if (value)
            onSetTrue.Invoke();
    }
}
