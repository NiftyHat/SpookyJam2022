using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterGuidePageEntry : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI monsterNameDisplay;
    [SerializeField]
    private UnityEngine.UI.Image monsterPortrait;
    [SerializeField]
    private TextMeshProUGUI descriptionDisplay;
    [SerializeField]
    private TextMeshProUGUI hintDisplay;




    //Populate page with monster data
    public void SetData(MonsterGuidePageSO data)
    {
        monsterNameDisplay.SetText(data.monsterName);
        monsterPortrait.sprite = data.image;
        descriptionDisplay.SetText(data.description);
        hintDisplay.SetText(data.hint);
    }

    public void SetDisplay( bool isVisible)
    {
        monsterNameDisplay.gameObject.SetActive(isVisible);
        monsterPortrait.gameObject.SetActive(isVisible);
        descriptionDisplay.gameObject.SetActive(isVisible);
        hintDisplay.gameObject.SetActive(isVisible);
    }
}
