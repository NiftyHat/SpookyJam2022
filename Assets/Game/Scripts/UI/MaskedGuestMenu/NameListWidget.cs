using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameListWidget : MonoBehaviour
{
    public NameEntry nameEntryPrefab;
    public Transform nameEntryContainer;

    public void Start()
    {
        string[] testData = { "Dinosaur", "Lettuce", "Cabbage" };
        Init(testData);
    }

    //Set up buttons
    public void Init(string[] data)
    {
        foreach(string name in data)
        {
            NameEntry nameEntry = GameObject.Instantiate<NameEntry>(nameEntryPrefab, nameEntryContainer);
            nameEntry.Init(name, this);
        }
    }

    public void OnDisable()
    {
        transform.SetParent(MaskedGuestCardWidget.SubmenuContainer);
    }


    public void OnNameSelected(string nameValue)
    {
        Debug.Log("YOYOI selected Name " + nameValue);
    }

}
