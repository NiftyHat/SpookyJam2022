using System;
using NiftyFramework.Core.Data;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class ColoredTextData
    {
        [SerializeField] private string _copy;
        [SerializeField] private Optional<Color> _color;
        public string Copy => _copy;
        public Optional<Color> Color => _color;
    }
}