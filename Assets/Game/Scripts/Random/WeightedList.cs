using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace RandomGenerator
{
    public class WeightedList
    {
        public interface IWeighted
        {
            public int Weight { get; }
        }

        protected int _total;
        protected readonly IList<IWeighted> _items;
        
        
        public WeightedList()
        {
            _items = new List<IWeighted>();
        }
        
        public bool Get<TData>(System.Random random, out TData itemWithWeight) where TData : class, IWeighted
        {
            return Get(random.Next(0, _total), out itemWithWeight);
        }

        public bool Get<TData>(int targetWeight, out TData itemWithWeight) where TData : class, IWeighted
        {
            if (_items == null)
            {
                itemWithWeight = default;
                return false;
            }
            if (targetWeight > _total)
            {
                targetWeight %= _total;
            }
            int count = 0;
            foreach (var item in _items)
            {
                count += item.Weight;
                if (targetWeight <= count)
                {
                    itemWithWeight = item as TData;
                    return true;
                }
            }
            if (count > _total)
            {
                Debug.LogError($"{nameof(NameTableData)}GetItem() over ran total item weight of {_total} with a count of {count}");
            }
            itemWithWeight = _items.Last() as TData;
            return true;
        }

        public void Remove(IWeighted weightedItem)
        {
            _items.Remove(weightedItem);
        }

        public void Add(IWeighted weightedItem)
        {
            _items.Add(weightedItem);
            _total += weightedItem.Weight;
        }

        public void Clear()
        {
            _items.Clear();
            _total = 0;
        }
    }
}