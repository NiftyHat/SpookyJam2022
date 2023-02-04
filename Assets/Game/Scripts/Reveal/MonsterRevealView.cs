using Entity;
using NiftyFramework.DataView;
using UnityEngine;

namespace Reveal
{
    public class MonsterRevealView : MonoBehaviour, IDataView<MonsterEntity>
    {
        [SerializeField] private SpriteRenderer _sprite;
    
        public void Clear()
        {
            gameObject.SetActive(false);
        }

        public void Set(MonsterEntity data)
        {
            if (data != null)
            {
                _sprite.sprite = data.TypeData.RevealSprite;
            }
        }
    }
}