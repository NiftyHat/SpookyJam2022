using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
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
            
            public Entry(CSVRecord csvRecord)
            {
                _first = ToTitleCase(csvRecord.FirstName);
                _last = ToTitleCase(csvRecord.LastName);
                _middle = csvRecord.MiddleName !=null ? ToTitleCase(csvRecord.MiddleName) : null;
                _gender = ParseGenderString(csvRecord.Gender);
                _name = $"{_first} {_last}";
            }

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
        
        public struct CSVRecord : IFactory<Entry>
        {
            [Name("firstName")]
            public string FirstName { get; set; }
            
            [Name("lastName")]
            public string LastName  { get; set; }
            
            [Name("middleName")][Optional]
            public string MiddleName  { get; set; }

            [Name("gender")] public string Gender  { get; set; }
            
            public Entry Create()
            {
                return new Entry(this);
            }
        }

        [SerializeField] private TextAsset _textAsset;
        [SerializeField] private List<Entry> _items;


        [ContextMenu("Import")]
        public void ImportCSV()
        {
            using (var reader = new StringReader(_textAsset.text))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = csv.GetRecord<CSVRecord>();
                    _items.Add(record.Create());
                }
            }
        }

        public void ImportCSV(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = csv.GetRecord<CSVRecord>();
                    _items.Add(record.Create());
                }
            }
        }
    }
}