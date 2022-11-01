using Data.Trait;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardUI
{
    public class TraitListWidget : MonoBehaviour
    {
        //@TODO Limit the total number of icons you can select
        [SerializeField]
        private Transform _container;

        [SerializeField]
        [Tooltip("List of location entry buttons except \'None\'")]
        private TraitEntryWidget _entryPrefab;
        [SerializeField]
        private TraitDataSet _traitData;

        [SerializeField]
        private TraitEntryWidget _noneToggle = null;
        [SerializeField]
        private List<TraitEntryWidget> _buttons = new List<TraitEntryWidget>();

        private MaskGuessCardWidget _parentMenuReference;


        //Generate Buttons for Menu
        public void Start()
        {
            _buttons.Clear();
            foreach (TraitData data in _traitData.References)
            {
                TraitEntryWidget button = GameObject.Instantiate<TraitEntryWidget>(_entryPrefab, _container);
                button.Initialize(data, false);
                _buttons.Add(button);
                button.onSetTrue.AddListener(OnNonNullEntrySelected);
            }
        }

        public void Initialize(List<TraitData> data, MaskGuessCardWidget parentMenu = null)
        {
            _parentMenuReference = parentMenu;

            _noneToggle.SetValue(data.Count == 0);

            foreach (TraitEntryWidget button in _buttons)
            {
                button.SetValue(data.Contains(button.Data));
            }
        }

        public List<TraitData> GetData()
        {
            List<TraitData> results = new List<TraitData>();
            foreach (TraitEntryWidget button in _buttons)
            {
                if (button.Value)
                    results.Add(button.Data);
            }
            return results;
        }


        public void OnEnable()
        {
            List<TraitData> test = GetData();
            foreach (TraitData data in test)
            {
                Debug.Log(" Location " + data.FriendlyName);
                Debug.Log(" Location " + data.FriendlyName);
            }
        }

        public void OnDisable()
        {
            _parentMenuReference = null;
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
                foreach (TraitEntryWidget button in _buttons)
                {
                    button.SetValue(false);
                }
            }
        }

        public void OnConfirmButtonPressed()
        {
            //Fuckin take the data and disable this menu
            _parentMenuReference.ConfirmTraitSubmenu();
        }
    }
}
