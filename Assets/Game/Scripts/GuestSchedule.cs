using System;
using System.Collections.Generic;
using System.Text;
using Data.Location;
using Generators;

namespace Interactions
{
    public class GuestSchedule
    {
        private readonly List<LocationData> _locationsByPhase;
        
        public GuestSchedule(System.Random random, List<ScheduleGenerator.WeightedLocationChance> weightedLocations)
        {
            _locationsByPhase = new List<LocationData>();
            for (int i = 0; i < weightedLocations.Count; i++)
            {
                var locationChance = weightedLocations[i];
                var locationData = locationChance.GetRandom(random);
                _locationsByPhase.Add(locationData);
            }
        }

        public bool TryGetLocation(int index, out LocationData location)
        {
            if (index >= 0 &&  index < _locationsByPhase.Count && _locationsByPhase[index] != null)
            {
                location = _locationsByPhase[index];
                return true;
            }
            location = null;
            return false;
        }

        public bool IsAtLocationDuringPhase(int index, Func<LocationData, bool> validator)
        {
            if (index >= 0 &&  index< _locationsByPhase.Count && _locationsByPhase[index] != null)
            {
                return validator(_locationsByPhase[index]);
            }
            return false;
        }
        

        public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _locationsByPhase.Count; i++)
            {
                if (TryGetLocation(i, out var locationData))
                {
                    sb.Append(i);
                    sb.Append(":");
                    sb.Append(locationData.FriendlyName);
                }
                else
                {
                    sb.Append("[NULL]");
                }

                if (i < _locationsByPhase.Count - 1)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }
    }
}