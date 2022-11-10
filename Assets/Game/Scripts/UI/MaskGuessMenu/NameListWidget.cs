using Context;
using Entity;
using NiftyFramework.Core.Context;
using System;
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

        private CharacterName _selectedNameData;

        public event Action<CharacterName> OnSelected;

        private void Start()
        {
            //Set up buttons with Guest List data
            List<CharacterName> data = new List<CharacterName>();
            GuestsData.With(characterEntity => data.Add(characterEntity.Name));

            foreach (CharacterName name in data)
            {
                NameEntry nameEntry = GameObject.Instantiate<NameEntry>(_nameEntryPrefab, _nameEntryContainer);
                nameEntry.Init(name, this);
            }
        }

        //Set up buttons
        public void Initialize(MaskGuessCardWidget maskGuessCardWidget)
        {
            //@TODO Highlight selected button 
        }

        public CharacterName GetData()
        {
            return _selectedNameData;
        }

        public void OnNameEntrySelected(CharacterName nameValue)
        {
            Debug.Log("YOYOI selected Name " + nameValue);
            //Same as Confirm Button for other submenus

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