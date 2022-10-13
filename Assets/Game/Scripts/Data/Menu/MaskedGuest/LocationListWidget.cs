using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationListWidget : MonoBehaviour
{
    public Transform container;

    [HideInInspector]
    [Tooltip("List of location entry buttons except \'None\'")]
    public GameObject locationEntryPrefab;

    [SerializeField]
    private LocationEntryWidget noneToggle = null;
    [SerializeField]
    private List<LocationEntryWidget> locations = new List<LocationEntryWidget>();


    public void Start()
    {
        foreach (LocationEntryWidget loc in locations)
        {
            loc.onSetTrue.AddListener(OnNonNullEntrySelected);
        }
    }

    public void Init(LocationsSeen data)
    {
        if ((data & LocationsSeen.None) == LocationsSeen.None)
            noneToggle.SetValue(true);

        foreach (LocationEntryWidget loc in locations)
        {
            loc.SetValue(data.HasFlag(loc.location));
        }
    }

    public LocationsSeen GetLocationData()
    {
        LocationsSeen result = LocationsSeen.None;
        foreach (LocationEntryWidget loc in locations)
        {
            if (loc.Value)
                result |= loc.location;
        }
        return result;
    }


    public void OnEnable()
    {
        LocationsSeen test = GetLocationData();
        foreach (LocationEntryWidget loc in locations)
        {
            Debug.Log(" Location " + loc.location.ToString() + " " + test.HasFlag(loc.location));
        }
    }



    public void OnNonNullEntrySelected()
    {
        //Set None Toggle to be accurate
        if(noneToggle.Value)
            noneToggle.SetValue(false);
    }

    //On selecting None, set all other location entries to false
    public void ClearEntries()
    {
        if (noneToggle.Value) //'None' set true
        {
            foreach (LocationEntryWidget location in locations)
            {
                location.SetValue(false);
            }
        }
    }
}
