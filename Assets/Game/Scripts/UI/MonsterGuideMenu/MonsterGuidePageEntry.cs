using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterGuidePageEntry : MonoBehaviour
{
    public TextMeshProUGUI monsterNameDisplay;
    public UnityEngine.UI.Image monsterPortrait;
    public TextMeshProUGUI descriptionDisplay;
    public TextMeshProUGUI hintDisplay;




    //Populate page with monster data
    public void SetData()
    {
        monsterNameDisplay.SetText("");
        monsterPortrait.sprite = null;
        descriptionDisplay.SetText("");
        hintDisplay.SetText("");
    }

    public void Display()
    {

    }

    public void Hide()
    {

    }
}
