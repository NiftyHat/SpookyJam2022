using Context;
using Data.Reactions;
using DG.Tweening;
using Entity;
using NiftyFramework.Core.Context;
using NiftyFramework.UI;
using TMPro;
using UI.Cards;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace UI
{
    public class UICharacterSelectPreview : MonoBehaviour, IView<CharacterEntity, ReactionData>
    {
        //[SerializeField] private UICardCharacterView _cardCharacterView;
        [SerializeField] private UICharacterView _character;
        [SerializeField] private TextMeshProUGUI _textFooter;
        [SerializeField] private UIReactionView _reactionView;
        [SerializeField] private Button _viewButton;
        [SerializeField] private UIGuessInfoView _guessInfo;

        private CharacterEntity _characterEntity;

        public void Start()
        {
            _viewButton.onClick.AddListener(HandleViewSelected);
            //transform.position = Vector3.left * 1200;
            Clear();
        }

        private void HandleViewSelected()
        {
            ContextService.Get<GameStateContext>(service =>
            {
                service.ShowCharacterReview(_characterEntity);
            });
        }

        public void Set(CharacterEntity characterEntity, ReactionData reactionData = null)
        {
            if (characterEntity == null)
            {
                Clear();
                return;
            }

            _characterEntity = characterEntity;
            if (gameObject.TrySetActive(true))
            {
                _character.Set(characterEntity);
                if (_textFooter != null)
                {
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
                }

                if (_guessInfo != null)
                {
                    _guessInfo.Set(characterEntity.TypeGuess);
                }
                
                if (reactionData != null)
                {
                    _reactionView.Set(reactionData);
                }
                else
                {
                    _reactionView.Clear();
                }
            }
           
        }

        public void Clear()
        {
            gameObject.TrySetActive(false);
        }
    }
}
