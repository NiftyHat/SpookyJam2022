using DG.Tweening;
using Entity;
using NiftyFramework.UI;
using UI.Cards;
using UnityEngine;

namespace UI
{
    public class UICharacterSelectPreview : MonoBehaviour, IView<CharacterEntity>
    {
        [SerializeField] private UICardCharacterView _cardCharacterView;

        public void Start()
        {
            //transform.position = Vector3.left * 1200;
            Clear();
        }
        public void Set(CharacterEntity viewData)
        {
            if (viewData == null)
            {
                Clear();
                return;
            }
            _cardCharacterView.SetFacingDown(false);
            transform.DOMoveY(292, 0.4f);
            _cardCharacterView.Set(viewData);
        }

        public void Clear()
        {
            _cardCharacterView.SetFacingDown(true);
            transform.DOMoveY(-1300, 0.4f);
        }
    }
}
