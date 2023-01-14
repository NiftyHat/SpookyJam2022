using System.Collections.Generic;
using System.Linq;
using Data.Reactions;
using Data.Trait;
using Entity;
using NiftyFramework.UI;
using TMPro;
using UI.Targeting;
using UnityEngine;
using UnityUtils;

namespace UI.Cards
{
    public class UICardCharacterView : MonoBehaviour, IView<CharacterEntity>
    {
        [SerializeField] private UICardHeaderView _cardHeader;
        [SerializeField] private TextMeshProUGUI _textFooter;
        [SerializeField] private CanvasGroup _cardBack;
        [SerializeField] private CanvasGroup _cardFront;
        [SerializeField] private UICharacterView _character;
        [SerializeField] private UITraitView[] _traitViewList;
        [SerializeField] private Animator _animator;
        [SerializeField] private UIGuessInfoView _guessInfo;
        private static readonly int FlipFaceDown = Animator.StringToHash("FlipFaceDown");
        private static readonly int SetFaceDown = Animator.StringToHash("SetFaceDown");
        private static readonly int SetFaceUp = Animator.StringToHash("SetFaceUp");

        public void Set(CharacterEntity characterEntity)
        {
            _character.Set(characterEntity);
            _character.SetHidden(!characterEntity.WasSeen.Value);
            if (characterEntity.Name != null)
            {
                if (characterEntity.Name.Full.Length <= 13)
                {
                    _textFooter.SetText(characterEntity.Name.Full);
                }
                else
                {
                    _textFooter.SetText($"{characterEntity.Name.First[0]}.{characterEntity.Name.Last}");
                }
            }
            else
            {
                _textFooter.SetText("???");
            }
            if (characterEntity.WasSeen.Value)
            {
                if (characterEntity.Mask != null)
                {
                    _cardHeader.Set(characterEntity.Mask.FriendlyName, characterEntity.Mask.CardValue);
                }
                else
                {
                    _cardHeader.Set("Guest");
                }
            }
            else
            {
                _cardHeader.Set("Unmet Guest");
            }
            _guessInfo.Set(characterEntity.TypeGuess);
        }

        public void FlipFacingDown(bool isFacingDown)
        {
            if (_animator != null)
            {
                _animator.SetBool(FlipFaceDown, isFacingDown);
            }
        }
        
        public void SetFacingDown(bool isFacingDown)
        {
            if (_animator != null)
            {
                _animator.SetBool(FlipFaceDown, isFacingDown);
                if (isFacingDown)
                {
                    _animator.SetTrigger(SetFaceDown);
                }
                else
                {
                    _animator.SetTrigger(SetFaceUp);
                }
            }
        }

        public void Clear()
        {
            if (_cardBack != null)
            {
                if (_cardBack.gameObject.TrySetActive(true))
                {
                    _cardFront.gameObject.TrySetActive(false);
                }
            }
        }
        
    }
}
