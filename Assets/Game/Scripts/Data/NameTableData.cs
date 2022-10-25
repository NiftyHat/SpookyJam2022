using System;
using System.Collections.Generic;
using System.Globalization;
using NiftyFramework.Core;
using NiftyFramework.Scripts;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "NameDataTable", menuName = "Game/Names/NameDataTable", order = 1)]
    public class NameTableData : ScriptableObject
    {
        public interface IWeighted
        {
            public int Weight { get; }
        }
        
        public class CSVRow
        {
            public string Copy { get; set; }
            public int Weight { get; set; }
            
            public bool IsValid()
            {
                return !string.IsNullOrEmpty(Copy);
            }
        }

        [Serializable]
        public class Entry : IWeighted
        {
            [SerializeField] protected string _copy;
            [SerializeField] protected int _weight;

            public string Copy => _copy;
            public int Weight => _weight;

            public Entry(string copy, int weight)
            {
                _copy = copy;
                _weight = weight;
            }

            public Entry(CSVRow item)
            {
                _copy = ToTitleCase(item.Copy);
                _weight = item.Weight;
            }
            
            public static string ToTitleCase(string str)
            {
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
            }
        }
        
        [SerializeField] private TextAsset _textAsset;
        [SerializeField] private List<Entry> _data;
        [SerializeField][ReadOnly] private int _totalWeight;

        [ContextMenu("Import")]
        public void Import()
        {
            /*
            if (_textAsset == null)
            {
                return;
            }
            
            var results = Read.Csv.FromString(_textAsset.text).With.ColumnsDelimiter(",")
                .ThatReturns.ArrayOf<CSVRow>()
                .Put.Column("name").Into(r => r.Copy)
                .Put.Column("weight").As<int>().Into(r => r.Weight)
                //.Put.Column("weight").As<int>().Into(e => e.Weight)
                .GetAll();

            _data = new List<Entry>();
            int totalWeight = 0;
            foreach (var item in results.ResultSet)
            {
                if (item != null && item.IsValid())
                {
                    totalWeight += item.Weight;
                    _data.Add(new Entry(item));
                }
            }

            _totalWeight = totalWeight;*/
        }

        [ContextMenu("SortWeight")]
        public void SortWeight()
        {
            _data.Sort((left, right) => left.Weight - right.Weight);
            _data.Reverse();
        }
        
        [ContextMenu("SortAlphabetical")]
        public void SortAlphabetical()
        {
            _data.Sort((left, right) => String.CompareOrdinal(left.Copy, right.Copy));
        }
        
        [ContextMenu("UpdateWeight")]
        public void UpdateWeight()
        {
            _totalWeight = 0;
            foreach (var item in _data)
            {
                _totalWeight += item.Weight;
            }
        }
        //TODO - add duplicate tolerance so we don't get too many of the same first name.
        public List<string> GetRandom(System.Random random, int count)
        {
            List<string> output = new List<string> ( new string[count] );
            int[] weights = ListUtils.GenerateInts(count, random, 0, _totalWeight);
            for (int i = 0; i < count; i++)
            {
                var randomEntry = GetItem(weights[i]);
                output[i] = randomEntry.Copy;
            }
            return output;
        }

        public Entry GetItem(int weight)
        {
            if (weight > _totalWeight)
            {
                weight %= _totalWeight;
            }
            int weightCount = 0;
            foreach (var data in _data)
            {
                if (weightCount >= weight)
                {
                    return data;
                }
                weightCount += data.Weight;
            }
            if (weightCount > _totalWeight)
            {
                Debug.LogError($"{nameof(NameTableData)}GetItem() over ran total item weight of {_totalWeight} with a count of {weightCount}");
            }
            return _data[_data.Capacity - 1];
        }
    }
}