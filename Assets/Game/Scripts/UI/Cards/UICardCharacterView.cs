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
        private static readonly int FaceDown = Animator.StringToHash("FaceDown");

        public void Set(CharacterEntity characterEntity)
        {
            _character.Set(characterEntity);
            if (characterEntity.Mask != null)
            {
                _cardHeader.Set(characterEntity.Mask.FriendlyName, characterEntity.Mask.CardValue);
            }
            else
            {
                _cardHeader.Set("Guest");
            }

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

            Dictionary<TraitData, Guess> traitGuesses = characterEntity.TraitGuessInfo;
            _guessInfo.Set(characterEntity.TypeGuess);
        }

        public void SetFacingDown(bool isFacingDown)
        {
            if (_animator != null)
            {
                _animator.SetBool(FaceDown, isFacingDown);
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
