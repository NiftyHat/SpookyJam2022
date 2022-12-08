using System;
using System.Collections.Generic;
using System.Linq;
using Data.Interactions;
using Data.Reactions;
using Data.Trait;
using DG.Tweening;
using NiftyFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityUtils;

namespace UI.Cards
{
    public class UICardTraitView : MonoBehaviour, IView<TraitData, IList<ReactionData>, IList<AbilityReactionTriggerData>>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private UICardHeaderView _cardHeader;
        [SerializeField] private TextMeshProUGUI _textFooter;
        [SerializeField] private CanvasGroup _cardBack;
        [SerializeField] private CanvasGroup _cardFront;
        [SerializeField] private UICardTraitContentContainer _content;
        [SerializeField] private UICardBorderView _boarderView;
        [SerializeField] private Animator _animator;
        [SerializeField] private Button _button;
        [SerializeField] private float _popHeight = 30;
        [SerializeField] private Canvas _canvas;

        public TraitData TraitData { get; private set; }
        public IList<ReactionData> ReactionList {get; private set; }
        public IList<AbilityReactionTriggerData> AbilityList { get; private set; }
 
        private Guess _guessValue;
        private bool _isFacingDown;
        //public event Action<UICardTraitView, bool> OnToggled;
        public event Action<UICardTraitView, Guess> OnGuessChange;

        private static readonly int FaceDown = Animator.StringToHash("FaceDown");

        protected int _defaultSortOrder;

        public void Set(TraitData traitData, IList<ReactionData> reactionDataList, IList<AbilityReactionTriggerData> abilityDataList)
        {
            if (_cardFront != null)
            {
                if (_cardFront.gameObject.TrySetActive(true))
                {
                    _cardBack.gameObject.TrySetActive(false);
                }
            }
            gameObject.SetActive(true);
            _content.Set(traitData, reactionDataList);
            _textFooter.SetText(traitData.FriendlyName);
            _cardHeader.Set(traitData.CardSuit, traitData.CardNumber);
            _button.onClick.AddListener(HandleClicked);
            ReactionList = reactionDataList;
            AbilityList = abilityDataList;
            TraitData = traitData;
            _defaultSortOrder = _canvas.renderOrder;
        }

        private void HandleClicked()
        {
            _guessValue = GuessUtils.Next(_guessValue);
            SetGuess(_guessValue);
            OnGuessChange?.Invoke(this,_guessValue);
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

        public void SetFacingDown(bool isFacingDown)
        {
            _isFacingDown = isFacingDown;
            if (_animator != null)
            {
                _animator.SetBool(FaceDown, isFacingDown);
            }

            if (isFacingDown)
            {
                _canvas.overrideSorting = false;
                _canvas.sortingOrder = _defaultSortOrder;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_animator != null && !_isFacingDown)
            {
                _animator.transform.DOLocalMoveY(_popHeight, 0.2f).SetEase(Ease.InCubic);
                _canvas.overrideSorting = true;
                _canvas.sortingOrder = 3;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_animator != null)
            {
                _animator.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.InCubic);
                _canvas.overrideSorting = false;
                _canvas.sortingOrder = _defaultSortOrder;
            }
        }

        public bool HasReaction(ReactionData reactionData)
        {
            return ReactionList.Contains(reactionData);
        }

        public bool HasAbility(AbilityReactionTriggerData ability)
        {
            return AbilityList.Contains(ability);
        }

        public void SetGuess(Guess guess)
        {
            _boarderView.Set(guess);
        }
    }
}
