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
        private Dictionary<MaskEntity, MaskGuessCardData> _maskGuessData = new Dictionary<MaskEntity, MaskGuessCardData>();


        public MaskGuessCardData GetData(MaskEntity mask)
        {
            MaskGuessCardData data;
            if (_maskGuessData.TryGetValue(mask, out data))
            {
                return data;
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
            _maskGuessData.Clear();
        }
    }
}
