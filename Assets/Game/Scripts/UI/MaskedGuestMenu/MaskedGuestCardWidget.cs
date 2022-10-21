using Data.Area;
using Data.Trait;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class MaskedGuestCardWidget : MonoBehaviour
{
    [SerializeField]
    private IconWidget iconPrefab;
    #region Icon Pooling
    private static IObjectPool<IconWidget> _iconPool;
    private static Transform poolContainer;
    public IObjectPool<IconWidget> IconPool
    {
        get
        {
            if (_iconPool == null)
            {
                GameObject poolContainer = new GameObject("UI ObjectPool - IconWidgets");
                _iconPool = new ObjectPool<IconWidget>(CreateIcon, actionOnGet: (obj) => obj.gameObject.SetActive(true), OnReleaseIcon, actionOnDestroy: (obj) => Destroy(obj), false, 6, 12);
            }
            return _iconPool;
        }
    }
    private IconWidget CreateIcon()
    {
        return GameObject.Instantiate<IconWidget>(iconPrefab);
    }
    private void OnReleaseIcon(IconWidget obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(poolContainer);
    }
    #endregion

    [SerializeField]
    private MaskedGuestCardData data;

    [Header("Display Data")]
    public Image guestPortrait;
    public Image maskPortrait;

    public TextMeshProUGUI nameDisplayText;

    public GameObject locationDisplay, traitDisplay;
    public Transform locationContainer, traitContainer;

    private List<IconWidget> locationIcons = new List<IconWidget>();
    private List<IconWidget> traitIcons = new List<IconWidget>();

    private void OnEnable()
    {
        Initialize(data);
    }

    public void Initialize(MaskedGuestCardData data)
    {
        this.data = data;
        maskPortrait.sprite = data.mask.CardSprite;
        maskPortrait.color = data.maskColor.Color;
        UpdateNameDisplay();
        UpdateLocationDisplay();
        UpdateTraitDisplay();
    }

    public void UpdateNameDisplay()
    {
        nameDisplayText.SetText(data.DisplayName);
    }

    public void UpdateLocationDisplay()
    {
        foreach (IconWidget icon in traitIcons)
        {
            IconPool.Release(icon);
        }
        traitIcons.Clear();
        foreach (TraitData trt in data.traitData)
        {
            IconWidget icon = IconPool.Get();
            icon.transform.SetParent(traitContainer);
            icon.SetSprite(trt.Icon);
            traitIcons.Add(icon);
        }
    }

    public void UpdateTraitDisplay()
    {
        foreach (IconWidget icon in locationIcons)
        {
            IconPool.Release(icon);
        }
        locationIcons.Clear();
        foreach (AreaData loc in data.locationData)
        {
            IconWidget icon = IconPool.Get();
            icon.transform.SetParent(locationContainer);
            icon.SetSprite(loc.Icon);
            locationIcons.Add(icon);
        }
    }

    public void ClearDisplay()
    {
        foreach (IconWidget icon in traitIcons)
        {
            IconPool.Release(icon);
        }
        traitIcons.Clear();
        foreach (IconWidget icon in locationIcons)
        {
            IconPool.Release(icon);
        }
        locationIcons.Clear();
    }

    public void ShowDetailedData(bool show)
    {
        locationDisplay.SetActive(show);
        traitDisplay.SetActive(show);
    }
}
