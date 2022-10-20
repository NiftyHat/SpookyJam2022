using Data.Area;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LocationEntryWidget : ToggleWidget<AreaData>
{
    public override void Initialize(AreaData data, bool value)
    {
        _data = data;
        toggle.image.sprite = data.Icon;
        label.text = data.FriendlyName;
        toggle.SetIsOnWithoutNotify(value);
    }
}
