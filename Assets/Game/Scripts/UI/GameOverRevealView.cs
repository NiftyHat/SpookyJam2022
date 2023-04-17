using System;
using Context;
using Data;
using Data.Monsters;
using Data.TextSequence;
using Entity;
using Febucci.UI;
using NiftyFramework;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using Reveal;
using TextSequence;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameOverRevealView : MonoBehaviour, IView<GameOverRevealView.Data>
    {
        public struct Data
        {
            public CharacterEntity Target;
            public MonsterEntityTypeData NearestMonster;
        }

        [SerializeField][NonNull] private Animator _animator;
        [SerializeField][NonNull] private MonsterRevealView _monsterView;
        [SerializeField][NonNull] private CharacterRevealView _characterView;
        [SerializeField] [NonNull] private AudioSource _musicSource;
        [SerializeField][NonNull] private TextSequencePlayerWidget _textSequencePlayer;
        [SerializeField][NonNull] private TextSequenceSetData _textSequenceWrongCharacter;
        [SerializeField][NonNull] private TextSequenceSetData _giveUp;
        [SerializeField] [NonNull] private TextMeshProUGUI _titleText;
        [SerializeField] private Sprite _killerSprite;
        
        [SerializeField][NonNull] private AudioClip _musicWin;
        [SerializeField][NonNull] private AudioClip _musicLose;
        
        [SerializeField] private ColoredTextData _titleLose;
        [SerializeField] private ColoredTextData _titleKilled;
        [SerializeField] private ColoredTextData _titleGiveUp;

        private static readonly int RevealState = Animator.StringToHash("RevealState");
        private static readonly int Exit = Animator.StringToHash("Exit");
        private static readonly int IsComplete = Animator.StringToHash("IsComplete");
        private object _handleAnimationComplete;

        private bool _isWaitingForInput = true;
        private bool _isAnimComplete = false;

        private GameStateContext _gameStateContext;

        public event Action OnComplete;

        public enum RevealAnimState
        {
            PickMonster = 1,
            PickPerson = 2,
            PickKiller = 3,
            GiveUp = 4
        }

        public void Awake()
        {
            _animator.enabled = false;
            ContextService.Get<GameStateContext>(HandleGameStateContext);
        }

        private void HandleGameStateContext(GameStateContext gameStateContext)
        {
            _gameStateContext = gameStateContext;
            if (_gameStateContext.GameOverState != null)
            {
                Set(_gameStateContext.GameOverState.RevealAnimData);
            }
        }

        public void Set(Data viewData)
        {
            SetCharacter(viewData.Target);
        }

        public void Clear()
        {
        }

        public void Start()
        {
            ContextService.Get<GameStateContext>(gameState =>
            {
                
               if (gameState.GameOverState != null)
               {
                   Set(gameState.GameOverState.RevealAnimData);
               }
               else
               {
                   SetCharacter(null);
               }
               /*
                var data = new Data()
                {
                    NearestMonster = null,
                    Target = gameState.CharacterEntities[0]
                };
                Set(data);*/
            });
        }

        public void SetCharacter(CharacterEntity targetCharacter)
        {
            if (targetCharacter != null)
            {
                if (_characterView != null)
                {
                    _characterView.Set(targetCharacter);
                }

                _textSequencePlayer.OnComplete += HandleTextSequenceComplete;
                if (targetCharacter is MonsterEntity monsterEntity)
                {
                    _musicSource.clip = _musicWin;
                    _musicSource.Play();
                    _textSequencePlayer.Set(monsterEntity.Name, monsterEntity.TypeData.RevealDialogue);
                    if (_monsterView != null)
                    {
                        _monsterView.Set(monsterEntity);
                    }
                    _animator.SetInteger(RevealState, (int)RevealAnimState.PickMonster);
                    _titleText.SetText(monsterEntity.TypeData.CopyWin.Copy);
                    _titleText.SetColor(monsterEntity.TypeData.CopyWin.Color);
                }
                else
                {
                    _musicSource.clip = _musicLose;
                    _musicSource.Play();
                    if (targetCharacter is KillerEntity)
                    {
                        _animator.SetInteger(RevealState, (int)RevealAnimState.PickKiller);
                        _titleText.SetText(_titleKilled.Copy);
                        _titleText.SetColor(_titleKilled.Color);
                    }
                    else
                    {
                        _animator.SetInteger(RevealState, (int)RevealAnimState.PickPerson);
                        _titleText.SetText(_titleLose.Copy);
                        _titleText.SetColor(_titleLose.Color);
                    }
                    _textSequencePlayer.Set(targetCharacter.Name, _textSequenceWrongCharacter);
                }
            }
            else
            {
                _musicSource.clip = _musicWin;
                _musicSource.Play();
                _textSequencePlayer.Set(_giveUp);
                if (_monsterView != null)
                {
                    _monsterView.Clear();
                }
                _animator.SetInteger(RevealState, (int)RevealAnimState.GiveUp);
                _titleText.SetText(_titleGiveUp.Copy);
                _titleText.SetColor(_titleGiveUp.Color);
            }
            _animator.enabled = true;
        }

        private void HandleTextSequenceComplete()
        {
            _isAnimComplete = true;
            _animator.SetBool(IsComplete, true);
        }

        public void Update()
        {
            if (_isWaitingForInput && Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                _isWaitingForInput = false;
                if (_isAnimComplete)
                {
                    OnComplete?.Invoke();
                    OnComplete = null;
                }
                else
                {
                    _animator.speed = 2;
                }
            }
        }
    }
}
