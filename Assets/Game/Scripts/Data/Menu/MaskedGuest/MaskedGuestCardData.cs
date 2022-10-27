using Data.Mask;
using Data.Style;
using Data.Trait;
using System;
using System.Collections;
using System.Collections.Generic;
using Data.Location;
using UnityEngine;
using Entity;

//Data object for Player's notes on masked guests
[Serializable]
public class MaskedGuestCardData
{
    public MaskEntity mask;
    public string DisplayName
    {
        get 
        {
            if (name == null)
                return mask.FriendlyName;
            else
                return name.Full;
        }
    }
    public Entity.CharacterName name;
    public List<LocationData> locationData;
    public List<TraitData> traitData;
}
