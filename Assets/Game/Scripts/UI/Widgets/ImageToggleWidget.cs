using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class ImageToggleWidget : MonoBehaviour, IView<bool>
    {
        [SerializeField][NonNull] private Image _icon;
        
        public void Set(bool isEnabled)
        {
            _icon.gameObject.SetActive(true);
        }

        public void Clear()
        {
            this.gameObject.SetActive(false);
        }
    }
}