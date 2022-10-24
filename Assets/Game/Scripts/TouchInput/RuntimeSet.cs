using System;
using System.Collections.Generic;
using UnityEngine;

namespace NiftyFramework
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        protected List<T> _itemList = new List<T>();

        public bool Add(T item)
        {
            if (!_itemList.Contains(item))
            {
                _itemList.Insert(0,item);
                return true;
            }
            return false;
        }

        public bool Remove(T item)
        {
            if (_itemList.Contains(item))
            {
                _itemList.Remove(item);
                return true;
            } 
            return false;
        }
        
        public bool Contains(T item) 
        {
            return _itemList.Contains(item);
        }

        public int Count () 
        {
            return _itemList.Count;
        }

        public void Clear ()
        {
            _itemList.Clear();
        }

        public void With (Action<T,int> deltaAction) {
            
            int len = _itemList.Count - 1;
            if (len >= 0)
            {
                for (int i = len; i >= 0; i--)
                {
                    T handler = _itemList[i];
                    deltaAction(handler,i);
                }
            }
        }

        public void With (Action<T> deltaAction) {
            
            int len = _itemList.Count - 1;
            if (len >= 0)
            {
                for (int i = len; i >= 0; i--)
                {
                    T handler = _itemList[i];
                    deltaAction(handler);
                }
            }
        }
    }
}

