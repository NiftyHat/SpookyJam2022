using System;
using System.Text;
using Data.Location;

public class GuestSchedule
{
    private readonly LocationData[] _orderedLocationList;
        
    public GuestSchedule(int phaseCount)
    {
        _orderedLocationList = new LocationData[phaseCount];
    }

    public void AddLocation(int phase, LocationData locationData)
    {
        if (phase < _orderedLocationList.Length)
        {
            _orderedLocationList[phase] = locationData;
        }
    }
    
    public bool TryGetLocation(int index, out LocationData location)
    {
        if (index >= 0 &&  index < _orderedLocationList.Length && _orderedLocationList[index] != null)
        {
            location = _orderedLocationList[index];
            return true;
        }
        location = null;
        return false;
    }

    public bool IsAtLocationDuringPhase(int index, Func<LocationData, bool> validator)
    {
        if (index >= 0 &&  index< _orderedLocationList.Length && _orderedLocationList[index] != null)
        {
            return validator(_orderedLocationList[index]);
        }
        return false;
    }
        

    public string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _orderedLocationList.Length; i++)
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

            if (i < _orderedLocationList.Length - 1)
            {
                sb.Append(",");
            }
        }
        return sb.ToString();
    }
}