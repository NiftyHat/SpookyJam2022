using Data;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using UnityEngine;

namespace Entity
{
    public class NPCEntityView : MonoBehaviour, IView<NPCEntity>
    {
        [SerializeField] protected Sprite _spriteMasc;
        [SerializeField] protected Sprite _spriteFemme;
        
        [SerializeField][NonNull] protected MaskEntityView _maskView;
        [SerializeField] protected SpriteRenderer _characterSpriteRender;

        protected NPCEntity _entity;
        
        public void Set(NPCEntity entity)
        {
            _entity = entity;

            switch (_entity.ImpliedGender)
            {
                case CharacterName.ImpliedGender.Femme:
                    _characterSpriteRender.sprite = _spriteFemme;
                    break;
                case CharacterName.ImpliedGender.Masc:
                    _characterSpriteRender.sprite = _spriteMasc;
                    break;
            }
            
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