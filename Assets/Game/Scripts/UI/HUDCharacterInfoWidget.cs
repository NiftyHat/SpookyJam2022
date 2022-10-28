using Entity;
using NiftyFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HUDCharacterInfoWidget : MonoBehaviour, IView<CharacterView>
    {
        [SerializeField] private Image _characterImage;
        [SerializeField] private Image _maskImage;
        
        public void Set(CharacterView viewData)
        {
        } 

        public void Clear()
        {
        }
    }
}