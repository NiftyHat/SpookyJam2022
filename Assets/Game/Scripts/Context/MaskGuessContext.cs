using Data;
using Entity;
using NiftyFramework.Core.Context;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Context
{
    public class MaskGuessContext : IContext
    {
        private Dictionary<MaskEntity, MaskGuessCardData> _maskGuessData;

        public void CreateNewEntry(MaskEntity mask)
        {
            if (!_maskGuessData.ContainsKey(mask))
            {
                MaskGuessCardData newData = new MaskGuessCardData(mask);
                _maskGuessData.Add(mask, newData);
            }
        }

        public void EditEntry(MaskGuessCardData data)
        {
            _maskGuessData[data.mask] = data;
        }

        public void Dispose()
        {
        }
    }
}
