using System;
using Data.GameOver;
using Data.Monsters;
using Entity;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using NiftyFramework.UnityUtils;
using UnityEngine;

namespace UI
{
    public class UIRevealAnimationView : MonoBehaviour, IDataView<GameOverReasonData, CharacterEntity, MonsterEntityTypeData>
    {
        [SerializeField][NonNull] private Animator _animator;
        [SerializeField][NonNull] private UIMonsterView _monsterView;
        [SerializeField][NonNull] private UICharacterView _characterView;

        private static readonly int RevealState = Animator.StringToHash("RevealState");
        private static readonly int Exit = Animator.StringToHash("Exit");

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
        }

        public void Clear()
        {
            _animator.SetTrigger(Exit);
        }

        public void Set(GameOverReasonData data, CharacterEntity targetCharacter, MonsterEntityTypeData monsterType)
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
        }
        
    }
}
