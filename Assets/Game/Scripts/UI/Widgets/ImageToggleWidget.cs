using NiftyFramework.Core;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class ImageToggleWidget: MonoBehaviour, IView<bool>, IView<ValueProvider<bool>>
    {
        [SerializeField][NonNull] private Image _icon;
        
        [SerializeField] private Sprite _spriteTrue;
        [SerializeField] private Sprite _spriteFalse;

        public void Set(bool isEnabled)
        {
            _icon.sprite = isEnabled ? _spriteTrue : _spriteFalse;
        }

        public void Set(ValueProvider<bool> viewData)
        {
            Set(viewData.Value);
            viewData.OnChanged += HandleMuteStateChanged;
        }

        private void HandleMuteStateChanged(bool newValue, bool oldValue)
        {
            Set(newValue);
        }

        public void Clear()
        {
            //this.gameObject.SetActive(false);
        }
    }
}