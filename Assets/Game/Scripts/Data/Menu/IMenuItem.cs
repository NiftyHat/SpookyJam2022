using NiftyFramework.Core.Data;
using UnityEngine;

namespace Data.Menu
{
    public interface IMenuItem
    {
        public string FriendlyName { get; }
        public Sprite Icon { get; }
        
        public IIconViewData SelectionIcon { get; }
        public string Description { get; }
    }
}