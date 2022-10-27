using Data.Mask;
using Data.Style;
using Data.Trait;
using System;
using System.Collections;
using System.Collections.Generic;
using Data.Location;
using UnityEngine;

//Data object for Player's notes on masked guests
[Serializable]
public class MaskedGuestCardData
{
    public MaskData mask;
    public ColorStyleData maskColor;
    public string DisplayName
    {
        get 
        {
            if (string.IsNullOrEmpty(name))
                return String.Format("{0} {1}", maskColor.FriendlyName, mask.FriendlyName);
            else
                return name;
        }
    }
    public string name;
    public List<LocationData> locationData;
    public List<TraitData> traitData;
}
