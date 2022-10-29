using Data.Monsters;
using NiftyFramework.DataView;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIMonsterView : MonoBehaviour, IDataView<MonsterEntityTypeData>
    {
        [SerializeField] private Image _image;
        
        public void Clear()
        {
            gameObject.SetActive(false);
        }

        public void Set(MonsterEntityTypeData data)
        {
            gameObject.SetActive(true);
            _image.sprite = data.RevealSprite;
        }
    }
}