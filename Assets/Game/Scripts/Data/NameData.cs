using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using UnityEngine;
using UnityUtils;

namespace Data
{
    [CreateAssetMenu(fileName = "NameData", menuName = "Game/PlayerData", order = 1)]
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
            [SerializeField] private string _first;
            [SerializeField] private string _middle;
            [SerializeField] private string _last;
            [SerializeField] private ImpliedGender  _gender;

            public string First => _first;
            public string Middle => _middle;
            public string Last => _last;
            
            public Entry(CSVRecord csvRecord)
            {
                _first = ToTitleCase(csvRecord.FirstName);
                _last = ToTitleCase(csvRecord.LastName);
                _middle = csvRecord.MiddleName !=null ? ToTitleCase(csvRecord.MiddleName) : null;
                _gender = ParseGenderString(csvRecord.Gender);
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
            public string FirstName;
            
            [Name("lastName")]
            public string LastName;
            
            [Name("middleName")]
            public string MiddleName;

            [Name("gender")] public string Gender;
            
            public Entry Create()
            {
                return new Entry(this);
            }
        }

        [SerializeField] private string _csvSourceRelativePath;

        public void ImportCSV(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
            };
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = csv.GetRecord<CSVRecord>();
                    // Do something with the record.
                }
            }
        }
    }
}