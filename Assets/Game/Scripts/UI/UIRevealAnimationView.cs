using System;
using Context;
using Data.Monsters;
using Entity;
using Interactions;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using UnityEngine;

namespace UI
{
    public class UIRevealAnimationView : MonoBehaviour, IDataView<CharacterEntity, MonsterEntityTypeData>
    {
        [SerializeField][NonNull] private Animator _animator;
        [SerializeField][NonNull] private UIMonsterView _monsterView;
        [SerializeField][NonNull] private UICharacterView _characterView;
        
        [SerializeField][NonNull] private AnimationDispatcher _animationDispatcher;
        [SerializeField][NonNull] private GameObject _inputPrompt;

        private static readonly int RevealState = Animator.StringToHash("RevealState");
        private static readonly int Exit = Animator.StringToHash("Exit");
        private object _handleAnimationComplete;

        private bool _isWaitingForInput = true;
        private bool _isAnimComplete = false;

        private GameStateContext _gameStateContext;

        public event Action OnComplete;

        public enum RevealAnimState
        {
            PickMonster = 1,
            PickPerson = 2,
            PickKiller = 3
        }

        public void Start()
        {
            gameObject.SetActive(false);
            _animator.enabled = false;
            ContextService.Get<GameStateContext>(HandleGameStateContext);
        }

        private void HandleGameStateContext(GameStateContext gameStateContext)
        {
            _gameStateContext = gameStateContext;
            _gameStateContext.OnConfessionConfirmed += HandleConfessionConfirmed;
        }

        private void HandleConfessionConfirmed(CharacterEntity entity, MonsterEntityTypeData monsterEntityType, Action onComplete)
        {
            Set(entity, monsterEntityType);
            OnComplete += onComplete;
        }

        public void Clear()
        {
            _animator.SetTrigger(Exit);
        }

        public void Set(CharacterEntity targetCharacter, MonsterEntityTypeData monsterType)
        {  
            gameObject.SetActive(true);
            _animator.enabled = true;
            if (_monsterView != null)
            {
                _monsterView.Set(monsterType);
            }
            if (_characterView != null)
            {
                _characterView.Set(targetCharacter);
            }
            switch (targetCharacter)
            {
                case MonsterEntity monsterEntity:
                    _animator.SetInteger(RevealState, (int)RevealAnimState.PickMonster);
                    break;
                case KillerEntity killerEntity:
                    _animator.SetInteger(RevealState, (int)RevealAnimState.PickKiller);
                    break;
                default:
                    _animator.SetInteger(RevealState, (int)RevealAnimState.PickPerson);
                    break;
            }

            _animationDispatcher.OnMessage += HandleAnimationComplete;
        }

        public void Update()
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
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

        private void HandleAnimationComplete(string obj)
        {
            _isAnimComplete = true;
            if (_isWaitingForInput)
            {
                _inputPrompt.gameObject.SetActive(true);
            }
            else
            {
                OnComplete?.Invoke();
                OnComplete = null;
            }
        }
    }
}
