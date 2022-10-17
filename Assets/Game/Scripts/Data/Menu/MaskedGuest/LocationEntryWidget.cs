using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LocationEntryWidget : MonoBehaviour
{
    public LocationsSeen location;
    public bool Value => toggle.isOn;
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private Text label;

    public UnityEvent onSetTrue;


    public void SetValue(bool value)
    {
        toggle.SetIsOnWithoutNotify(value);
        if (value)
            onSetTrue.Invoke();
    }
}
