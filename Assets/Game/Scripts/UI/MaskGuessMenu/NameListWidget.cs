using Context;
using Entity;
using NiftyFramework.Core.Context;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardUI
{
    public class NameListWidget : MonoBehaviour
    {
        public CharacterEntitySet GuestsData;
        [SerializeField] private NameEntry _nameEntryPrefab;
        [SerializeField] private Transform _nameEntryContainer;

        private MaskGuessCardWidget _parentMenuReference;

        private CharacterName _selectedNameData;

        private bool _isInit = false;

        //Set up buttons
        public void Initialize(MaskGuessCardWidget parentMenu = null)
        {
            _parentMenuReference = parentMenu;

            if (!_isInit)
            {
                List<CharacterName> data = new List<CharacterName>();
                GuestsData.With(characterEntity => data.Add(characterEntity.Name));

                foreach (CharacterName name in data)
                {
                    NameEntry nameEntry = GameObject.Instantiate<NameEntry>(_nameEntryPrefab, _nameEntryContainer);
                    nameEntry.Init(name, this);
                }
                _isInit = true;
            }
        }

        public void OnDisable()
        {
            _parentMenuReference = null;
        }


        public void OnNameEntrySelected(CharacterName nameValue)
        {
            Debug.Log("YOYOI selected Name " + nameValue);
            //Same as Confirm Button for other submenus
            _parentMenuReference.ConfirmNameSubmenu();
        }

        public CharacterName GetData()
        {
            return _selectedNameData;
        }
    }
}