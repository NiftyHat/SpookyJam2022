using Data.Trait;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardUI
{
    public class TraitEntryWidget : ToggleWidget<TraitData>
    {
        public override void Initialize(TraitData data, bool value)
        {
            _data = data;
            toggle.image.sprite = data.Icon;
            label.text = data.FriendlyName;
            toggle.SetIsOnWithoutNotify(value);
        }
    }
}
