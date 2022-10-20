using Data.Area;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationListWidget : MonoBehaviour
{
    [SerializeField]
    private Transform container;

    [SerializeField]
    [Tooltip("List of location entry buttons except \'None\'")]
    private LocationEntryWidget locationEntryPrefab;
    [SerializeField]
    private AreaDataSet locationData; 

    [SerializeField]
    private LocationEntryWidget noneToggle = null;
    [SerializeField]
    private List<LocationEntryWidget> locationButtons = new List<LocationEntryWidget>();


    //Generate Buttons for Menu
    public void Start()
    {
        locationButtons.Clear();
        foreach (AreaData location in locationData.References)
        {
            LocationEntryWidget locEntry = GameObject.Instantiate<LocationEntryWidget>(locationEntryPrefab, container);
            locEntry.Initialize(location, false);
            locationButtons.Add(locEntry);
            locEntry.onSetTrue.AddListener(OnNonNullEntrySelected);
        }
    }

    public void Init(List<AreaData> data)
    {
        noneToggle.SetValue(data.Count == 0);

        foreach (LocationEntryWidget loc in locationButtons)
        {
            loc.SetValue(data.Contains(loc.LocationData));
        }
    }

    public List<AreaData> GetLocationData()
    {
        List<AreaData> results = new List<AreaData>();
        foreach (LocationEntryWidget loc in locationButtons)
        {
            if (loc.Value)
                results.Add(loc.LocationData);
        }
        return results;
    }


    public void OnEnable()
    {
        List<AreaData> test = GetLocationData();
        foreach (AreaData loc in test)
        {
            Debug.Log(" Location " + loc.GetFriendlyName());
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
            foreach (LocationEntryWidget location in locationButtons)
            {
                location.SetValue(false);
            }
        }
    }
}
