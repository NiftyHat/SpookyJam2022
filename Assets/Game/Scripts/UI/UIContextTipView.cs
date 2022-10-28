using Data;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIContextTipView : MonoBehaviour, IView<string, IIconViewData>
    {
        [SerializeField][NonNull] private TextMeshProUGUI _labelCopy;
        [SerializeField][NonNull] private IconWidget _iconWidget;
    
        public void Set(string copy, IIconViewData iconViewData)
        {
            _labelCopy.SetText(copy);
            if (iconViewData.Sprite != null)
            {
                _iconWidget.gameObject.SetActive(true);
                _iconWidget.Set(iconViewData);
            }
            else
            {
                _iconWidget.gameObject.SetActive(false);
            }
        
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}
