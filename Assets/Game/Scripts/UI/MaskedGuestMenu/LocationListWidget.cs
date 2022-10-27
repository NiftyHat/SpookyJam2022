using System.Collections;
using System.Collections.Generic;
using Data.Location;
using UnityEngine;
using UnityEngine.UI;

public class LocationListWidget : MonoBehaviour
{
    //@TODO Limit the total number of icons you can select
    [SerializeField]
    private Transform container;

    [SerializeField]
    [Tooltip("List of location entry buttons except \'None\'")]
    private LocationEntryWidget entryPrefab;
    [SerializeField]
    private LocationDataSet locationData; 

    [SerializeField]
    private LocationEntryWidget noneToggle = null;
    [SerializeField]
    private List<LocationEntryWidget> buttons = new List<LocationEntryWidget>();

    private MaskedGuestCardWidget parentMenuReference;


    //Generate Buttons for Menu
    public void Start()
    {
        buttons.Clear();
        foreach (LocationData data in locationData.References)
        {
            LocationEntryWidget button = GameObject.Instantiate<LocationEntryWidget>(entryPrefab, container);
            button.Initialize(data, false);
            buttons.Add(button);
            button.onSetTrue.AddListener(OnNonNullEntrySelected);
        }
    }

    public void Init(List<LocationData> data, MaskedGuestCardWidget parentMenu = null)
    {
        parentMenuReference = parentMenu;

        noneToggle.SetValue(data.Count == 0);

        foreach (LocationEntryWidget button in buttons)
        {
            button.SetValue(data.Contains(button.Data));
        }
    }

    public List<LocationData> GetData()
    {
        List<LocationData> results = new List<LocationData>();
        foreach (LocationEntryWidget button in buttons)
        {
            if (button.Value)
                results.Add(button.Data);
        }
        return results;
    }


    public void OnEnable()
    {
        //Debug Stuff
        List<LocationData> test = GetData();
        foreach (LocationData data in test)
        {
            Debug.Log(" Location " + data.FriendlyName);
        }
    }

    public void OnDisable()
    {
        parentMenuReference = null;
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
            foreach (LocationEntryWidget location in buttons)
            {
                location.SetValue(false);
            }
        }
    }

    public void OnConfirmButtonPressed()
    {
        //Fuckin take the data bro and disable this menu dog
        parentMenuReference.ConfirmLocationSubmenu();
    }
}
