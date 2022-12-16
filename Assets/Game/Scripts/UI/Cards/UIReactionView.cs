using Data.Reactions;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace UI.Cards
{
    public class UIReactionView : MonoBehaviour, IView<ReactionData>
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _label;

        public void Set(ReactionData viewData)
        {
            if (viewData != null && gameObject.TrySetActive(true))
            {
                _image.sprite = viewData.Sprite;
                if (_background != null)
                {
                    _background.sprite = viewData.Background;
                }
                if (_label != null && _label.gameObject.TrySetActive(false))
                {
                    _label.SetText(viewData.FriendlyName);
                }
            }
        }

        public void Clear()
        {
            gameObject.TrySetActive(false);
            if (_label != null)
            {
                _label.gameObject.TrySetActive(false);
            }
        }
    }
}
