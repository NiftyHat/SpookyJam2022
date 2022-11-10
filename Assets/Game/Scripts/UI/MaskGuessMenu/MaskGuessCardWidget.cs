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
using NiftyFramework.Core.Utils;

namespace CardUI //NFT Hat no more yell at me
{
    public class MaskGuessCardWidget : MonoBehaviour
    {
        #region UI controls
        [SerializeField] private GameObject closeButton;
        #endregion


        #region Icon Pooling    
        [SerializeField] private IconWidget iconPrefab;
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

        [SerializeField] [NonNull] private NameListWidget _nameListWidget;
        [SerializeField] [NonNull] private LocationListWidget _locationListWidget;
        [SerializeField] [NonNull] private TraitListWidget _traitListWidget;

        #endregion


        [SerializeField]
        private MaskGuessCardData data = null;
        private MaskEntity _selectedMask;

        [Header("Display Data")]
        public Image guestPortrait;
        public Image maskPortrait;

        public TextMeshProUGUI nameDisplayText;

        public GameObject locationDisplay, traitDisplay;
        public Transform locationContainer, traitContainer;

        private List<IconWidget> locationIcons = new List<IconWidget>();
        private List<IconWidget> traitIcons = new List<IconWidget>();

        public void SetData(MaskEntity selectedMask)
        {
            _selectedMask = selectedMask;
        }

        public void ShowSingleWidget()
        {
            if (_selectedMask == null)
                return;

            Initialize(data);
            closeButton.SetActive(true);
            this.gameObject.SetActive(true);
        }

        public void CloseSingleWidget()
        {
            closeButton.SetActive(false);
            this.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            Initialize(data);
        }

        public void Initialize(MaskGuessCardData data)
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
            if (_nameListWidget.isActiveAndEnabled)
                return;

            Debug.Log("Show Name Menu");
            _nameListWidget.transform.position = this.transform.position;
            _nameListWidget.gameObject.SetActive(true);
            _nameListWidget.Initialize(this);
            _nameListWidget.OnSelected += HandleNameSelected;
        }

        public void HandleNameSelected(CharacterName nameSelected)
        {
            data.name = nameSelected;
            UpdateTraitDisplay();
            _nameListWidget.OnSelected -= HandleNameSelected;
        }

        public void ShowTraitSubmenu()
        {
            if (_traitListWidget.isActiveAndEnabled)
                return;

            Debug.Log("Show Trait Menu");
            _traitListWidget.transform.position = this.transform.position;
            _traitListWidget.gameObject.SetActive(true);
            _traitListWidget.Initialize(data.traitData);
            _traitListWidget.OnSelected += HandleTraitsSelected;
        }

        private void HandleTraitsSelected(List<TraitData> traitsSelected)
        {
            data.traitData = traitsSelected;
            UpdateTraitDisplay();
            _traitListWidget.OnSelected -= HandleTraitsSelected;
        }

        public void ShowLocationSubmenu()
        {
            Debug.Log("Show Location Menu");
            _locationListWidget.transform.position = this.transform.position;
            _locationListWidget.gameObject.SetActive(true);
            _locationListWidget.Initialize(data.locationData);
            _locationListWidget.OnSelected += HandleLocationsSelected;
        }


        public void HandleLocationsSelected(List<LocationData> locationsSelected)
        {
            data.locationData = locationsSelected;
            UpdateLocationDisplay();
            _locationListWidget.OnSelected -= HandleLocationsSelected;
        }


        #endregion
    }
}