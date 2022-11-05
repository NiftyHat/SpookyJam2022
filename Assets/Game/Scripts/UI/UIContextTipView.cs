using Context;
using Data;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIContextTipView : MonoBehaviour, IView<string, IIconViewData>, IView<ITooltip>
    {
        [SerializeField][NonNull] private TextMeshProUGUI _labelCopy;
        [SerializeField][NonNull] private IconWidget _iconWidget;

        public void Start()
        {
            ContextService.Get<TooltipContext>(HandleTooltipContext);
        }

        private void HandleTooltipContext(TooltipContext service)
        {
            service.OnChange += OnTooltipChange;
        }

        private void OnTooltipChange(ITooltip tip)
        {
            Set(tip);
        }

        public void Set(string copy, IIconViewData iconViewData)
        {
            if (copy != null)
            {
                gameObject.SetActive(true);
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
        }

        public void Set(ITooltip tooltip)
        {
            if (tooltip == null)
            {
                Clear();
                return;
            }
            string copy = tooltip.GetCopy();
            IIconViewData iconViewData = tooltip.GetIcon();
            
            Set(copy, iconViewData);
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}
