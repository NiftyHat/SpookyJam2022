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


        public MaskGuessCardData GetData(MaskEntity mask)
        {
            if (_maskGuessData == null)
            {
                _maskGuessData = new Dictionary<MaskEntity, MaskGuessCardData>();
            }

            if (_maskGuessData.ContainsKey(mask))
            {
                return _maskGuessData[mask];
            }
            else
            {
                MaskGuessCardData newData = new MaskGuessCardData(mask);
                _maskGuessData.Add(mask, newData);
                return newData;
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
