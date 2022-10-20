using Data.Area;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LocationEntryWidget : MonoBehaviour
{
    [SerializeField]
    private AreaData locationData;
    public AreaData LocationData => locationData;

    public bool Value => toggle.isOn;
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private Text label;

    public UnityEvent onSetTrue;

    public void Initialize(AreaData data, bool value)
    {
        locationData = data;
        toggle.image.sprite = data.GetSprite();
        label.text = data.GetFriendlyName();
        toggle.SetIsOnWithoutNotify(value);
    }

    public void SetValue(bool value)
    {
        toggle.SetIsOnWithoutNotify(value);
        if (value)
            onSetTrue.Invoke();
    }
}
