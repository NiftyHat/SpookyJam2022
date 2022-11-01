using System;
using System.Linq;
using Context;
using Data.Interactions;
using Entity;
using Interactions;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using UnityEngine;

namespace UI.Targeting
{
    public class UISelectedTargetView : MonoBehaviour, IDataView<ITargetable>
    {
        [SerializeField][NonNull] private UITargetPortraitPanel _targetPortrait;
        [SerializeField][NonNull] private UIInteractionListPanel _interactionList;
        [SerializeField][NonNull] private UIAssignedTraitsPanel _assignedTraitsPanel;
        private GameStateContext _gameStateContext;
        private PlayerInputHandler _player;
        public event Action<InteractionState> OnPreviewInteraction;

        public void Start()
        {
            ContextService.Get<GameStateContext>(HandleGameStateContext);
            _interactionList.OnPreviewInteraction += HandlePreviewInteraction;
            Clear();
        }

        private void HandlePreviewInteraction(InteractionState interactionState)
        {
            OnPreviewInteraction?.Invoke(interactionState);
        }

        private void HandleGameStateContext(GameStateContext gameStateContext)
        {
            _gameStateContext = gameStateContext;
            _gameStateContext.GetPlayer(player => _player = player);
        }

        public void Clear()
        {
            _interactionList.Clear();
            _targetPortrait.Clear();
            _assignedTraitsPanel.Clear();
        }

        public void Set(ITargetable target)
        {
            if (target is ITargetable<CharacterEntity> selectableCharacter)
            {
                var instance = selectableCharacter.GetInstance();
                if (instance == null)
                {
                    Clear();
                    return;
                }
                _targetPortrait.Set(instance);
                if (instance.Traits != null)
                {
                    _assignedTraitsPanel.Set(instance.Traits.ToList());
                }
                else
                {
                    _assignedTraitsPanel.Clear();
                }
            }
            if (_gameStateContext != null)
            {
                bool FilterInteractions(InteractionData interaction)
                {
                    if (interaction.IsValidTarget(target))
                    {
                        return true;
                    }
                    return false;
                }
                var interactions = _gameStateContext.GetInteractions(FilterInteractions);
                var targetingInfo = new TargetingInfo(_player, target);
                _interactionList.Set(interactions, targetingInfo);
            }
        }
    }
}
