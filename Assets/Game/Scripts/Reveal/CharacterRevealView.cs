using Entity;
using NiftyFramework.UI;
using UnityEngine;

namespace Reveal
{
    public class CharacterRevealView : MonoBehaviour, IView<CharacterEntity>
    {
        [SerializeField] private MaskEntityView _maskView;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private CharacterEntity _entity;
        public CharacterEntity Entity => _entity;
        
        public void Set(CharacterEntity entity)
        {
            if (_entity != null)
            {
                Clear();
            }
            _entity = entity;
            _spriteRenderer.sprite = _entity.ViewData.Sprite;
            _maskView.Set(_entity.Mask);
        }

        public void LateUpdate()
        {
            if (_entity != null && _entity is not KillerEntity)
            {
                _spriteRenderer.sprite = _entity.ViewData.Sprite;
            }
            //Debug.Log($"name sprite: {  _entity?.ViewData.Sprite}");
        }

        public void Clear()
        {
            _spriteRenderer.sprite = null;
            if (_maskView != null)
            {
                _maskView.Clear();
            }
        }
    }
}