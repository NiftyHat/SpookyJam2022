using Data;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using UnityEngine;

namespace Entity
{
    public class NPCEntityView : MonoBehaviour, IView<CharacterEntity>
    {
        [SerializeField][NonNull] protected MaskEntityView _maskView;
        [SerializeField] protected SpriteRenderer _characterSpriteRender;

        protected CharacterEntity _entity;
        
        public void Set(CharacterEntity entity)
        {
            _entity = entity;

            _characterSpriteRender.sprite = _entity.ViewData.Sprite;
            
            if (entity.Name.Full != null)
            {
                name = entity.Name.Full;
            }

            if (entity.Mask == null)
            {
                Debug.LogError($"An NPC '{name}' has no mask. This is bad.");
            }
            if (_maskView != null)
            {
                _maskView.Set(entity.Mask);
            }
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}