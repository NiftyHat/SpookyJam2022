using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameListWidget : MonoBehaviour
{
    public NameEntry nameEntryPrefab;
    public Transform nameEntryContainer;

    private MaskedGuestCardWidget parentMenuReference;

    public void Start()
    {
        string[] testData = { "Dinosaur", "Lettuce", "Cabbage" };
        Init(testData);
    }

    //Set up buttons
    public void Init(string[] data, MaskedGuestCardWidget parentMenu = null)
    {
        parentMenuReference = parentMenu;

        foreach (string name in data)
        {
            NameEntry nameEntry = GameObject.Instantiate<NameEntry>(nameEntryPrefab, nameEntryContainer);
            nameEntry.Init(name, this);
        }
    }

    public void OnDisable()
    {
        parentMenuReference = null;
    }


    public void OnNameSelected(string nameValue)
    {
        Debug.Log("YOYOI selected Name " + nameValue);
        //Same as Confirm Button for other submenus
    }

}
