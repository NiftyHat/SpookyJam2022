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
                if (data.Mask != null)
                {
                    _mask.gameObject.SetActive(true);
                    _mask.sprite = data.Mask.MaskData.WorldSprite;
                    _mask.color = data.Mask.ColorStyleData.Color;
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