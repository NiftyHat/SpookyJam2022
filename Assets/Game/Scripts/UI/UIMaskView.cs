using Data.Mask;
using Entity;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIMaskView : MonoBehaviour, IView<MaskEntity>
    {
        [SerializeField] protected MaskData _maskData;
        [SerializeField][NonNull] protected Image _image;

        public void Start()
        {
            _image.sprite = _maskData.WorldSprite;
        }

        public MaskData MaskData => _maskData;

        [UnityEngine.ContextMenu("InitWithData")]
        public void InitWithData()
        {
            _image.sprite = _maskData.WorldSprite;
            name = _maskData.FriendlyName;
        }

        public void Set(MaskEntity viewData)
        {
            if (viewData == null)
            {
                Clear();
                return;
            }
            gameObject.SetActive(true);
            _image.sprite = viewData.MaskData.WorldSprite;
            _image.color = viewData.Color;
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}