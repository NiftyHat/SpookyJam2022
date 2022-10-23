using Data.Trait;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitListWidget : MonoBehaviour
{

    [SerializeField]
    private Transform container;

    [SerializeField]
    [Tooltip("List of location entry buttons except \'None\'")]
    private TraitEntryWidget entryPrefab;
    [SerializeField]
    private TraitDataSet traitData;

    [SerializeField]
    private TraitEntryWidget noneToggle = null;
    [SerializeField]
    private List<TraitEntryWidget> buttons = new List<TraitEntryWidget>();


    //Generate Buttons for Menu
    public void Start()
    {
        buttons.Clear();
        foreach (TraitData data in traitData.References)
        {
            TraitEntryWidget button = GameObject.Instantiate<TraitEntryWidget>(entryPrefab, container);
            button.Initialize(data, false);
            buttons.Add(button);
            button.onSetTrue.AddListener(OnNonNullEntrySelected);
        }
    }

    public void Init(List<TraitData> data)
    {
        noneToggle.SetValue(data.Count == 0);

        foreach (TraitEntryWidget button in buttons)
        {
            button.SetValue(data.Contains(button.Data));
        }
    }

    public List<TraitData> GetData()
    {
        List<TraitData> results = new List<TraitData>();
        foreach (TraitEntryWidget button in buttons)
        {
            if (button.Value)
                results.Add(button.Data);
        }
        return results;
    }


    public void OnEnable()
    {
        List<TraitData> test = GetData();
        foreach (TraitData data in test)
        {
            Debug.Log(" Location " + data.FriendlyName);
            Debug.Log(" Location " + data.FriendlyName);
        }
    }

    public void OnDisable()
    {
        transform.SetParent(MaskedGuestCardWidget.SubmenuContainer);
    }


    public void OnNonNullEntrySelected()
    {
        //Set None Toggle to be accurate
        if (noneToggle.Value)
            noneToggle.SetValue(false);
    }

    //On selecting None, set all other location entries to false
    public void ClearEntries()
    {
        if (noneToggle.Value) //'None' set true
        {
            foreach (TraitEntryWidget button in buttons)
            {
                button.SetValue(false);
            }
        }
    }
}
