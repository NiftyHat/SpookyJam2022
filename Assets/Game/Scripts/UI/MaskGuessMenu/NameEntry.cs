using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Entity;

namespace CardUI
{
    public class NameEntry : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public CharacterName nameValue;
        public Button button;
        private NameListWidget nameListReference;


        public void Init(CharacterName name, NameListWidget nameList)
        {
            nameText.SetText(name.Full);
            nameValue = name;
            nameListReference = nameList;
        }

        public void SelectName()
        {
            nameListReference.OnNameEntrySelected(nameValue);
        }
    }
}
