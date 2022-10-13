using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NameEntry : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Button button;

    public void Start()
    {
        //button.onClick.AddListener();
    }



    public void SetName(string name)
    {
        nameText.SetText(name);
    }
}
