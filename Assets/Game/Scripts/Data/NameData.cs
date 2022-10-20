using System;
using System.Collections.Generic;
using System.Globalization;

using UnityEngine;
using UnityUtils;

namespace Data
{
    [CreateAssetMenu(fileName = "NameData", menuName = "Game/NameData", order = 1)]
    public class NameData : ScriptableObject
    {
        public enum ImpliedGender
        {
            None,
            Masculine,
            Feminine,
        }
        
        [Serializable]
        public struct Entry
        {
            [SerializeField] private string _name;
            [SerializeField] private string _first;
            [SerializeField] private string _middle;
            [SerializeField] private string _last;
            [SerializeField] private ImpliedGender  _gender;

            public string First => _first;
            public string Middle => _middle;
            public string Last => _last;

            public string Name => _name;
            
            private static ImpliedGender ParseGenderString(string csvRecordGender)
            {
                switch (csvRecordGender)
                {
                    case "m":
                        return ImpliedGender.Masculine;
                    case "f":
                        return ImpliedGender.Feminine;
                    default:
                        return ImpliedGender.None;
                }
            }
            
            public static string ToTitleCase(string str)
            {
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
            }
        }
        
        [SerializeField] private TextAsset _textAsset;
        [SerializeField] private List<Entry> _items;
        
    }
}