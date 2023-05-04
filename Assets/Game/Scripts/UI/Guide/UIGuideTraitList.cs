using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Trait;
using NiftyFramework.UI;
using NiftyFramework.UnityUtils;
using UnityEngine;

namespace UI.Guide
{
    public class UIGuideTraitList : MonoBehaviour, IView<TraitDataSet>
    {
        [SerializeField] private TraitDataSet _traitDataSet;
        [SerializeField] private PlayerData _playerData;
        
        private List<UIGuideTrait> _traitViews;
        //private MonoPool<UIGuideTrait> _traitViewPool;
        

        private void Start()
        {
            _traitViews = GetComponentsInChildren<UIGuideTrait>().ToList();
            Set(_traitDataSet);
        }

        public void Set(TraitDataSet traitDataSet)
        {
            for (int i = 0; i < _traitViews.Count; i++)
            {
                if (i < _traitViews.Count && _traitViews[i] != null)
                {
                    if (i < _traitDataSet.References.Count && _traitDataSet.References[i] != null)
                    {
                        _traitViews[i].Set(_traitDataSet.References[i], _playerData);
                    }
                }
                else
                {
                    throw new NullReferenceException();
                }
                
            }
        }

        public void Clear()
        {
            
        }
    }
}