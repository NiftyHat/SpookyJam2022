using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class NameEntry : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public string nameValue;
    public Button button;
    private NameListWidget nameListReference;


    public void Init(string name, NameListWidget nameList)
    {
        nameText.SetText(name);
        nameValue = name;
        nameListReference = nameList;
    }

    public void SelectName()
    {
        nameListReference.OnNameSelected(nameValue);
    }
}
