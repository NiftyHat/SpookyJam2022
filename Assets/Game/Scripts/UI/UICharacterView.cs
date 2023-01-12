using Entity;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICharacterView : MonoBehaviour, IDataView<CharacterEntity>
    {
        [SerializeField][NonNull] private Image _character;
        [SerializeField][NonNull] private Image _mask;

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
                _character.sprite = data.ViewData.Sprite;
                //AlignPivot(_character);
                
                if (data.Mask != null)
                {
                    _mask.gameObject.SetActive(true);
                    _mask.sprite = data.Mask.MaskData.WorldSprite;
                    _mask.color = data.Mask.ColorStyleData.Color;
                    _mask.SetNativeSize();
                    
                    //AlignPivot(_mask);
                }
                else
                {
                    _mask.gameObject.SetActive(false);
                }
            }
            else
            {
                Clear();
            }
        }
    }
}