using System;
using System.Collections.Generic;
using Data.Location;
using UnityEngine;

namespace CardUI
{
    public class LocationListWidget : MonoBehaviour
    {
        //@TODO Limit the total number of icons you can select
        [SerializeField] private Transform _container;

        [SerializeField]
        [Tooltip("List of location entry buttons except \'None\'")]
        private LocationEntryWidget _entryPrefab;
        [SerializeField]
        private LocationDataSet _locationData;

        [SerializeField]
        private LocationEntryWidget _noneToggle = null;
        [SerializeField]
        private List<LocationEntryWidget> _buttons = new List<LocationEntryWidget>();

        public event Action<List<LocationData>> OnSelected;

        //Generate Buttons for Menu
        public void Start()
        {
            _buttons.Clear();
            foreach (LocationData data in _locationData.References)
            {
                LocationEntryWidget button = GameObject.Instantiate<LocationEntryWidget>(_entryPrefab, _container);
                button.Initialize(data, false);
                _buttons.Add(button);
                button.onSetTrue.AddListener(OnNonNullEntrySelected);
            }
        }

        public void Initialize(List<LocationData> data)
        {
            _noneToggle.SetValue(data.Count == 0);

            foreach (LocationEntryWidget button in _buttons)
            {
                button.SetValue(data.Contains(button.Data));
            }
        }

        public List<LocationData> GetData()
        {
            List<LocationData> results = new List<LocationData>();
            foreach (LocationEntryWidget button in _buttons)
            {
                if (button.Value)
                    results.Add(button.Data);
            }
            return results;
        }


        public void OnEnable()
        {
            //Debug Stuff
            List<LocationData> test = GetData();
            foreach (LocationData data in test)
            {
                Debug.Log(" Location " + data.FriendlyName);
            }
        }


        public void OnNonNullEntrySelected()
        {
            //Set None Toggle to be accurate
            if (_noneToggle.Value)
                _noneToggle.SetValue(false);
        }

        //On selecting None, set all other location entries to false
        public void ClearEntries()
        {
            if (_noneToggle.Value) //'None' set true
            {
                foreach (LocationEntryWidget location in _buttons)
                {
                    location.SetValue(false);
                }
            }
        }

        public void OnConfirmButtonPressed()
        {
            var data = GetData();
            OnSelected?.Invoke(data);
            Close();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
