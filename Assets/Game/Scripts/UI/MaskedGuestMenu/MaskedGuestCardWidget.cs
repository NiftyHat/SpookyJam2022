using Data.Area;
using Data.Trait;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaskedGuestCardWidget : MonoBehaviour
{
    [SerializeField]
    private IconWidget iconPrefab;
    private MaskedGuestCardData data;

    [Header("Display Data")]
    public Image guestPortrait;

    public TextMeshProUGUI nameDisplayText;

    public Transform locationContainer, traitContainer;

    private List<IconWidget> locationIcons;
    private List<IconWidget> traitIcons;


    public void Initialize(MaskedGuestCardData data)
    {
        this.data = data;
        nameDisplayText.SetText(data.name);
        foreach (AreaData loc in data.locationData)
        {
            IconWidget icon = GameObject.Instantiate<IconWidget>(iconPrefab, locationContainer);
            icon.SetSprite(loc.Icon);
        }
        foreach (TraitData trt in data.traitData)
        {
            IconWidget icon = GameObject.Instantiate<IconWidget>(iconPrefab, traitContainer);
            icon.SetSprite(trt.Icon);
        }
    }

    public void ShowDetailedData(bool show)
    {

    }

}
