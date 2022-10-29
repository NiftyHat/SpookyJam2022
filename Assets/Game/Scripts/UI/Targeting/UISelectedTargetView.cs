using System;
using System.Linq;
using Context;
using Data.Interactions;
using Entity;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using UnityEngine;

namespace UI.Targeting
{
    public class UISelectedTargetView : MonoBehaviour, IDataView<CharacterEntity>
    {
        [SerializeField][NonNull] private UITargetPortraitPanel _targetPortrait;
        [SerializeField][NonNull] private UIInteractionListPanel _interactionList;
        [SerializeField][NonNull] private UIAssignedTraitsPanel _assignedTraitsPanel;
        private GameStateContext _gameStateContext;

        public void Start()
        {
            ContextService.Get<GameStateContext>(HandleGameStateContext);
            Clear();
        }

        private void HandleGameStateContext(GameStateContext gameStateContext)
        {
            _gameStateContext = gameStateContext;
        }

        public void Clear()
        {
            _interactionList.Clear();
            _targetPortrait.Clear();
            _assignedTraitsPanel.Clear();
        }

        public void Set(CharacterEntity entity)
        {
            if (entity == null)
            {
                Clear();
                return;
            }
            _targetPortrait.Set(entity);
            if (entity.Traits != null)
            {
                _assignedTraitsPanel.Set(entity.Traits.ToList());
            }
            else
            {
                _assignedTraitsPanel.Clear();
            }

            if (_gameStateContext != null)
            {
                bool FilterInteractions(InteractionData interaction)
                {
                    if (interaction.IsTargetType(InteractionData.TargetType.Other))
                    {
                        return true;
                    }
                    return false;
                }
                var interactions = _gameStateContext.GetInteractions(FilterInteractions);
                _interactionList.Set(interactions);
            }
        }
    }
}
