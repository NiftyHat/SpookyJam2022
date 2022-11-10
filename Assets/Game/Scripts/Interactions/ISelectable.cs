using System;

namespace Interactions
{
    public interface ISelectable
    {
        bool IsSelected { get; }
        public void SetSelected(bool state);
    }
}