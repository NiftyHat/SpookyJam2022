using System;
using Data.Trait;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardUI
{
    public class TraitEntryWidget : ToggleWidget<TraitData>
    {
        public event Action<bool> OnToggleChanged;
        
        public override void Initialize(TraitData data, bool value)
        {
            _data = data;
            toggle.image.sprite = data.Icon;
            label.text = data.FriendlyName;
            toggle.SetIsOnWithoutNotify(value);
            toggle.onValueChanged.AddListener(HandleValueChanged);
        }

        private void HandleValueChanged(bool value)
        {
            OnToggleChanged?.Invoke(value);
        }
    }
}
