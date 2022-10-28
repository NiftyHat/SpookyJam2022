using NiftyFramework.Core.Data;
using UnityEngine;

namespace Data
{
    public interface IIconViewData
    {
        public Sprite Sprite { get; }
        public Optional<Color> Tint { get; }
    }
}