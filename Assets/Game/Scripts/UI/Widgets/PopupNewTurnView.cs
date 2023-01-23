using System;
using Context;
using NiftyFramework.Core.Context;
using NiftyFramework.UnityUtils;
using TMPro;
using UI.Audio;
using UnityEngine;

namespace UI.Screens
{
    public class PopupNewTurnView : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected TextMeshProUGUI _textPhase;
        [SerializeField] protected AnimatedTimeLabelView _textTime;
        [SerializeField] protected TextMeshProUGUI _textSubtitle;
        [SerializeField] protected TurnChangeAudio _turnChangeAudio;

        protected GameStateContext _gameStateContext;
        protected int _animationIn = Animator.StringToHash("UIPopupNewTurn@In");
        protected int _animationOut = Animator.StringToHash("UIPopupNewTurn@Out");

        public void Start()
        {
            ContextService.Get<GameStateContext>(HandleGameStateContext);
        }

        private void HandleGameStateContext(GameStateContext service)
        {
            _gameStateContext = service;
            
            _textTime.Set(service.CurrentTime);
            HandleTurnStarted(service.Turns.Value, service.Turns.Max, service.Phase.Value);

            service.OnTurnStarted += HandleTurnStarted;
            service.OnPhaseChange += HandlePhaseChange;

            StateChangeDispatchBehaviour stateChangeDispatchBehaviour = _animator.GetBehaviour<StateChangeDispatchBehaviour>();
            if (stateChangeDispatchBehaviour != null)
            {
                stateChangeDispatchBehaviour.OnStateExited += HandleStateExit;
            }
        }

        private void HandlePhaseChange(int newValue, int oldValue)
        {
            if (newValue != oldValue)
            {
                _turnChangeAudio.PlayPhaseChange();
            }
        }

        private void HandleStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
        {
        }

        private void HandleTurnStarted(int turn, int turnMax, int phase)
        {
            TimeSpan subtractTime = TimeSpan.FromMinutes(5);
            int turnsRemaining = turnMax - turn;
            _textPhase.text = AddOrdinal(phase + 1) + " Phase";
            _textSubtitle.text = $"{turnsRemaining} turns remain until Midnight";
            _textTime.Set(_gameStateContext.CurrentTime - subtractTime, _gameStateContext.CurrentTime, 3f);
            _animator.Play(_animationIn);
            gameObject.SetActive(true);
            _turnChangeAudio.PlayTurnEnd();
        }

        public static string AddOrdinal(int num)
        {
            if( num <= 0 ) return num.ToString();

            switch(num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }
    
            switch(num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }
        }
    }
}
