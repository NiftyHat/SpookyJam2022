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
        toggle.image.sprite = data.GetSprite();
        label.text = data.GetFriendlyName();
        toggle.SetIsOnWithoutNotify(value);
    }
}
