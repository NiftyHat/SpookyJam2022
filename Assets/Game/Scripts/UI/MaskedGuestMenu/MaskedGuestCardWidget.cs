using System.Collections.Generic;
using Data.Location;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using CardUI;
using Data.Trait;
using Data;
using Entity;

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
                poolContainer = new GameObject("UI ObjectPool - IconWidgets").transform;
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

    #region Sub Menu References 
    private static Transform _submenuContainer;
    public static Transform SubmenuContainer
    {
        get
        {
            if (_submenuContainer == null)
            {
                _submenuContainer = new GameObject("MaskedGuestCard - Submenus").transform;
            }
            return _submenuContainer;
        }
    }

    [SerializeField] private NameListWidget nameListWidgetPrefab;
    private static NameListWidget _nameListWidget;
    public NameListWidget NameListSubmenu
    {
        get
        {
            if (_nameListWidget == null)
            {
                _nameListWidget = GameObject.Instantiate<NameListWidget>(nameListWidgetPrefab, SubmenuContainer);
            }
            return _nameListWidget;
        }
    }

    [SerializeField] private LocationListWidget locationListPrefab;
    private static LocationListWidget _locationListWidget;
    public LocationListWidget LocationListSubmenu
    {
        get
        {
            if (_locationListWidget == null)
            {
                _locationListWidget = GameObject.Instantiate<LocationListWidget>(locationListPrefab, SubmenuContainer);
            }
            return _locationListWidget;
        }
    }

    [SerializeField] private TraitListWidget traitListPrefab;
    private static TraitListWidget _traitListWidget;
    public TraitListWidget TraitListSubmenu
    {
        get
        {
            if (_traitListWidget == null)
            {
                _traitListWidget = GameObject.Instantiate<TraitListWidget>(traitListPrefab, SubmenuContainer);
            }
            return _traitListWidget;
        }
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
        maskPortrait.sprite = data.mask.MaskData.CardSprite;
        maskPortrait.color = data.mask.Color;
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
        foreach (IconWidget icon in locationIcons)
        {
            IconPool.Release(icon);
        }
        locationIcons.Clear();
        foreach (LocationData loc in data.locationData)
        {
            IconWidget icon = IconPool.Get();
            icon.transform.SetParent(locationContainer);
            icon.SetSprite(loc.Icon);
            locationIcons.Add(icon);
        }
    }

    public void UpdateTraitDisplay()
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

    #region Submenu Handling

    public void ShowNameSubmenu()
    {
        Debug.Log("Show Name Menu");
        NameListSubmenu.gameObject.SetActive(true);
        NameListSubmenu.transform.SetParent(this.transform);
        NameListSubmenu.transform.position = this.transform.position;

        NameListSubmenu.Initialize(this);
    }

    public void ShowTraitSubmenu()
    {
        Debug.Log("Show Trait Menu");
        TraitListSubmenu.gameObject.SetActive(true);
        TraitListSubmenu.transform.SetParent(this.transform);
        TraitListSubmenu.transform.position = this.transform.position;
        TraitListSubmenu.Initialize(data.traitData, this);
    }

    public void ShowLocationSubmenu()
    {
        Debug.Log("Show Location Menu");
        LocationListSubmenu.gameObject.SetActive(true);
        LocationListSubmenu.transform.SetParent(this.transform);
        LocationListSubmenu.transform.position = this.transform.position;
        LocationListSubmenu.Initialize(data.locationData, this);
    }

    public void ConfirmTraitSubmenu()
    {
        data.traitData = TraitListSubmenu.GetData();
        TraitListSubmenu.gameObject.SetActive(false);
        TraitListSubmenu.transform.SetParent(SubmenuContainer);
        UpdateTraitDisplay();
    }

    public void ConfirmLocationSubmenu()
    {
        data.locationData = LocationListSubmenu.GetData();
        LocationListSubmenu.gameObject.SetActive(false);
        LocationListSubmenu.transform.SetParent(SubmenuContainer);
        UpdateLocationDisplay();
    }

    public void ConfirmNameSubmenu()
    {
        CharacterName name = NameListSubmenu.GetData();
        if (name != null)
            data.name = NameListSubmenu.GetData();
        NameListSubmenu.gameObject.SetActive(false);
        NameListSubmenu.transform.SetParent(SubmenuContainer);

    }

    #endregion
}
