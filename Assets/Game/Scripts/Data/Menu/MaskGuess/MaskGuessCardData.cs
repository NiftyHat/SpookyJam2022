using Data.Mask;
using Data.Style;
using Data.Trait;
using System;
using System.Collections;
using System.Collections.Generic;
using Data.Location;
using UnityEngine;
using Entity;

namespace Data
{
    //Data object for Player's notes on masked guests
    [Serializable]
    public class MaskGuessCardData
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
        public CharacterName name;
        public List<LocationData> locationData;
        public List<TraitData> traitData;

        public MaskGuessCardData(MaskEntity mask)
        {
            this.mask = mask;
        }

    }
}
