using System;
using System.Collections.Generic;
using NiftyFramework.Core;

using UnityEngine;
using UnityUtils;


namespace NiftyFramework
{
    [ExecuteInEditMode]
    public abstract class ViewContainer<TMonoComp> : MonoBehaviour where TMonoComp : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private TMonoComp[] _views;
        private TMonoComp _activeItem;

        protected TMonoComp GetView(int index)
        {
            if (index < _views.Length)
            {
                return _views[index];
            }
            else
            {
                if (_views != null)
                {
                    Debug.LogError($"{nameof(ViewContainer<TMonoComp>)}() requested view index {index} out of range of {nameof(_views)} length {_views.Length}");
                }
                else
                {
                    Debug.LogError($"{nameof(ViewContainer<TMonoComp>)}() requested view index {index} but {nameof(_views)} is null");
                }
            }
            return null;
        }

        protected void OnValidate()
        {
            if (!Application.isPlaying)
            {
                SetViewsFromGameObject();
            }
            
        }

        protected void SetViewsFromGameObject()
        {
            var items= GetComponentsInChildren<TMonoComp>(true);
            List<TMonoComp> orderedItems = new List<TMonoComp>();
            foreach (var item in items)
            {
                int index = item.gameObject.transform.GetSiblingIndex();
                orderedItems.Insert(index, item);
            }
            _views = orderedItems.ToArray();
        }

        protected void SetActive(TMonoComp item)
        {
            if (_activeItem != item && _activeItem != null)
            {
                _activeItem.gameObject.TrySetActive(false);
            }
            _activeItem = item;
            _activeItem.gameObject.SetActive(true);
        }
        
        public void Update()
        {
            if (!Application.isPlaying)
            {
                foreach (var item in _views)
                {
                    if (item.isActiveAndEnabled && item != _activeItem)
                    {
                        if (_activeItem != null)
                        {
                            _activeItem.gameObject.SetActive(false);
                        }
                        _activeItem = item;
                        _activeItem.gameObject.SetActive(true);
                    }
                }
            }
            
        }
    }
}