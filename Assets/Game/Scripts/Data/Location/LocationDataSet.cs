using NiftyScriptableSet;
using UnityEngine;

namespace Data.Location
{
    [CreateAssetMenu(fileName = "LocationDataSet", menuName = "Game/World/LocationDataSet", order = 3)]
    public class LocationDataSet : ScriptableSet<LocationData>
    {
    }
}