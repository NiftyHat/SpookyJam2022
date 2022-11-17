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
using System;
using Context;
using NiftyFramework.Core.Context;

namespace CardUI //NFT Hat no more yell at me
{
    public class MaskGuessCardWidget : MonoBehaviour
    {

        #region UI controls
        [SerializeField] private GameObject _closeButton;
        #endregion


        #region Icon Pooling    
        [SerializeField] private IconWidget _iconPrefab;
        private static IObjectPool<IconWidget> _iconPool;
        private static Transform _poolContainer;
        public IObjectPool<IconWidget> IconPool
        {
            get
            {
                if (_iconPool == null)
                {
                    _poolContainer = new GameObject("UI ObjectPool - IconWidgets").transform;
                    _iconPool = new ObjectPool<IconWidget>(CreateIcon, actionOnGet: (obj) => obj.gameObject.SetActive(true), OnReleaseIcon, actionOnDestroy: (obj) => Destroy(obj), false, 6, 12);
                }
                return _iconPool;
            }
        }
        private IconWidget CreateIcon()
        {
            return GameObject.Instantiate<IconWidget>(_iconPrefab);
        }
        private void OnReleaseIcon(IconWidget obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_poolContainer);
        }
        #endregion


        #region Sub Menu References 

        [SerializeField] [NonNull] private NameListWidget _nameListWidget;
        [SerializeField] [NonNull] private LocationListWidget _locationListWidget;
        [SerializeField] [NonNull] private TraitListWidget _traitListWidget;

        #endregion


        [SerializeField]
        private MaskGuessCardData _data = null;
        private MaskEntity _selectedMask;
        private MaskGuessContext _maskGuessContext;

        [Header("Display Data")]
        [SerializeField] [NonNull] private Image _maskPortrait;

        [SerializeField] [NonNull] private TextMeshProUGUI _nameDisplayText;

        [SerializeField] [NonNull] private GameObject _locationDisplay, _traitDisplay;
        [SerializeField] [NonNull] private Transform _locationContainer, _traitContainer;

        private List<IconWidget> _locationIcons = new List<IconWidget>();
        private List<IconWidget> _traitIcons = new List<IconWidget>();


        public event Action<MaskGuessCardData> OnConfirm;


        public void CloseSingleWidget()
        {
            _closeButton.SetActive(false);
            this.gameObject.SetActive(false);
        }

        private void Start()
        {
            ContextService.Get<MaskGuessContext>(HandleMaskGuessContext);
        }

        private void HandleMaskGuessContext(MaskGuessContext service)
        {
            _maskGuessContext = service;
        }


        public void Initialize(MaskEntity selectedMask)
        {
            this._data = _maskGuessContext.GetData(selectedMask);
            _maskPortrait.sprite = _data.mask.MaskData.CardSprite;
            _maskPortrait.color = _data.mask.Color;
            UpdateNameDisplay();
            UpdateLocationDisplay();
            UpdateTraitDisplay();
        }

        public void UpdateNameDisplay()
        {
            _nameDisplayText.SetText(_data.DisplayName);
        }

        public void UpdateLocationDisplay()
        {
            foreach (IconWidget icon in _locationIcons)
            {
                IconPool.Release(icon);
            }
            _locationIcons.Clear();
            foreach (LocationData loc in _data.locationData)
            {
                IconWidget icon = IconPool.Get();
                icon.transform.SetParent(_locationContainer);
                icon.SetSprite(loc.Icon);
                _locationIcons.Add(icon);
            }
        }

        public void UpdateTraitDisplay()
        {
            foreach (IconWidget icon in _traitIcons)
            {
                IconPool.Release(icon);
            }
            _traitIcons.Clear();
            foreach (TraitData trt in _data.traitData)
            {
                IconWidget icon = IconPool.Get();
                icon.transform.SetParent(_traitContainer);
                icon.SetSprite(trt.Icon);
                _traitIcons.Add(icon);
            }
        }


        public void ClearDisplay()
        {
            foreach (IconWidget icon in _traitIcons)
            {
                IconPool.Release(icon);
            }
            _traitIcons.Clear();
            foreach (IconWidget icon in _locationIcons)
            {
                IconPool.Release(icon);
            }
            _locationIcons.Clear();
        }

        public void ShowDetailedData(bool show)
        {
            _locationDisplay.SetActive(show);
            _traitDisplay.SetActive(show);
        }


        public void HandleConfirmButtonPressed()
        {
            OnConfirm?.Invoke(_data);
        }


        private void SaveMaskGuessData()
        {
            _maskGuessContext.EditEntry(_data);
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
            _data.name = nameSelected;
            UpdateTraitDisplay();
            _nameListWidget.OnSelected -= HandleNameSelected;
            SaveMaskGuessData();
        }

        public void ShowTraitSubmenu()
        {
            if (_traitListWidget.isActiveAndEnabled)
                return;

            Debug.Log("Show Trait Menu");
            _traitListWidget.transform.position = this.transform.position;
            _traitListWidget.gameObject.SetActive(true);
            _traitListWidget.Initialize(_data.traitData);
            _traitListWidget.OnConfirm += HandleTraitsSelected;
        }

        private void HandleTraitsSelected(List<TraitData> traitsSelected)
        {
            _data.traitData = traitsSelected;
            UpdateTraitDisplay();
            _traitListWidget.OnConfirm -= HandleTraitsSelected;
            SaveMaskGuessData();
        }

        public void ShowLocationSubmenu()
        {
            Debug.Log("Show Location Menu");
            _locationListWidget.transform.position = this.transform.position;
            _locationListWidget.gameObject.SetActive(true);
            _locationListWidget.Initialize(_data.locationData);
            _locationListWidget.OnSelected += HandleLocationsSelected;
        }


        public void HandleLocationsSelected(List<LocationData> locationsSelected)
        {
            _data.locationData = locationsSelected;
            UpdateLocationDisplay();
            _locationListWidget.OnSelected -= HandleLocationsSelected;
            SaveMaskGuessData();
        }


        #endregion
    }
}