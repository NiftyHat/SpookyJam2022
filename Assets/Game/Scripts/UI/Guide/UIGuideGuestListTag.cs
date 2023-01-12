using Data.Mask;
using Entity;
using NiftyFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace UI.Guide
{
    public class UIGuideGuestListTag : MonoBehaviour, IView<MaskEntity>
    {
        [SerializeField] private Image _maskImage;
        [SerializeField] private Transform _unknownIcon;

        public void Set(MaskEntity maskEntity)
        {
            if (maskEntity != null)
            {
                _unknownIcon.gameObject.TrySetActive(false);
                if (_maskImage.gameObject.TrySetActive(true))
                {
                    _maskImage.sprite = maskEntity.MaskData.IconSprite;
                    _maskImage.color = maskEntity.ColorStyleData.Color;
                }
            }
            else
            {
                _unknownIcon.gameObject.TrySetActive(true);
                _maskImage.gameObject.TrySetActive(false);
            }
        }

        public void Clear()
        {
            _unknownIcon.gameObject.TrySetActive(true);
            _maskImage.gameObject.TrySetActive(false);
        }
    }
}