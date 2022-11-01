using Data;
using NiftyFramework.Core.Context;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Context
{
    public class MaskGuessContext : IContext
    {
        private List<MaskGuessCardData> _maskGuessData;

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
