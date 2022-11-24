using System;
using System.Collections.Generic;
using System.Linq;
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
    public class UICardTraitView : MonoBehaviour, IView<TraitData, IList<ReactionData>>, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private UICardHeaderView _cardHeader;
        [SerializeField] private TextMeshProUGUI _textFooter;
        [SerializeField] private CanvasGroup _cardBack;
        [SerializeField] private CanvasGroup _cardFront;
        [SerializeField] private UICardTraitContentContainer _content;
        [SerializeField] private Animator _animator;
        [SerializeField] private Button _button;
        [SerializeField] private float _popHeight = 30;
        [SerializeField] private Canvas _canvas;

        public TraitData TraitData { get; private set; }
        public IList<ReactionData> ReactionList {get; private set; }

        private bool _isTabbedOut;
        private bool _isFacingDown;
        public event Action<UICardTraitView, bool> OnToggled;

        private static readonly int FaceDown = Animator.StringToHash("FaceDown");

        protected int _defaultSortOrder;

        public void Set(TraitData traitData, IList<ReactionData> reactionDataList)
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
            TraitData = traitData;
            _defaultSortOrder = _canvas.renderOrder;
        }

        private void HandleClicked()
        {
            ToggleTabbedOut();
            OnToggled?.Invoke(this,_isTabbedOut);
        }

        private void ToggleTabbedOut()
        {
            SetTabbedOut(!_isTabbedOut);
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

        public void SetTabbedOut(bool isTabbedOut)
        {
            if (_isTabbedOut != isTabbedOut)
            {
                _isTabbedOut = isTabbedOut;
                if (_isTabbedOut == false)
                {
                    _animator.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.InCubic);
                    _canvas.overrideSorting = false;
                    _canvas.sortingOrder = _defaultSortOrder;

                }
                else
                {
                    _animator.transform.DOLocalMoveY(_popHeight, 0.2f).SetEase(Ease.InCubic);

                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_animator != null && !_isFacingDown)
            {
                _animator.transform.DOLocalMoveY(_popHeight, 0.2f).SetEase(Ease.InCubic);
                _canvas.overrideSorting = true;
                _canvas.sortingOrder = 1;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_animator != null && !_isTabbedOut)
            {
                _animator.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.InCubic);
                _canvas.overrideSorting = false;
                _canvas.sortingOrder = _defaultSortOrder;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ToggleTabbedOut();
            OnToggled?.Invoke(this,_isTabbedOut);
        }

        public bool HasReaction(ReactionData reactionData)
        {
            return ReactionList.Contains(reactionData);
        }
    }
}
