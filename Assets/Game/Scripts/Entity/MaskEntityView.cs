using NiftyFramework.UI;
using UnityEngine;

namespace Entity
{
    public class MaskEntityView : MonoBehaviour, IView<MaskEntity>
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void Set(MaskEntity data)
        {
            if (data != null)
            {
                gameObject.SetActive(true);
                _spriteRenderer.color = data.Color;
                _spriteRenderer.sprite = data.MaskData.WorldSprite;
            }
            else
            {
                gameObject.SetActive(false); 
            }
            
        }

        public void Clear()
        {
            _spriteRenderer.color = Color.magenta;
            _spriteRenderer.sprite = null;
            gameObject.SetActive(false);
        }
    }
}
