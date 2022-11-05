using System.Collections;
using System.Collections.Generic;
using Data.Location;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CardUI
{
    public class LocationEntryWidget : ToggleWidget<LocationData>
    {
        public override void Initialize(LocationData data, bool value)
        {
            _data = data;
            toggle.image.sprite = data.Icon;
            label.text = data.FriendlyName;
            toggle.SetIsOnWithoutNotify(value);
        }
    }
}
