using Entity;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace UI
{
    public class UICharacterView : MonoBehaviour, IDataView<CharacterEntity>
    {
        [SerializeField][NonNull] private Image _character;
        [SerializeField] private UIMaskView[] _maskViews;
        [SerializeField] private Transform _makeViewHolder;

        public void Clear()
        {
            gameObject.SetActive(false);
        }

        static void AlignPivot(Image image)
        {
  
            RectTransform imageRect = image.GetComponent<RectTransform>();
            Vector2 size = imageRect.sizeDelta;
            Vector2 pixelPivot = image.sprite.pivot;
            Vector2 percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
            imageRect.pivot = percentPivot;
        }

        public void SetHidden(bool isHidden)
        {
            if (isHidden)
            {
                _makeViewHolder.gameObject.SetActive(false);
                _character.color = Color.black;
            }
            else
            {
                _makeViewHolder.gameObject.SetActive(true);
                _character.color = Color.white;
            }
        }

        public void SetMask(MaskEntity mask)
        {
            foreach (var item in _maskViews)
            {
                if (item.MaskData == mask.MaskData)
                {
                    item.Set(mask);
                }
                else
                {
                    item.Clear();
                }
            }
        }
        
        public void Set(CharacterEntity data)
        {
            if (data == null)
            {
                Clear();
                return;
            }

            if (data.ViewData != null)
            {
                gameObject.SetActive(true);
                _character.gameObject.SetActive(true);
                _character.sprite = data.ViewData.UISprite;
                if (data.Mask != null)
                {
                    SetMask(data.Mask);
                }
                else
                {
                    SetMask(null);
                }
            }
            else
            {
                Clear();
            }
        }
    }
}