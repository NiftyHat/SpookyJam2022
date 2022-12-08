using System.Collections.Generic;
using Data.Location;
using Interactions;
using UI;
using UnityEngine;

namespace Data.Interactions
{
    public class FastTravelInteractionData : SubMenuInteractionData
    {
        [SerializeField] private LocationDataSet _locationDataSet;
        
        public override void Init()
        {
            throw new System.NotImplementedException();
        }

        protected override List<IInteraction> GetSubCommands()
        {
            throw new System.NotImplementedException();
        }
    }
}