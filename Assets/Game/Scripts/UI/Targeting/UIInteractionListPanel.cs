using System;
using System.Collections.Generic;
using System.Linq;
using Context;
using Data.Interactions;
using Entity;
using Interactions;
using Interactions.Commands;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using NiftyFramework.UnityUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace UI.Targeting
{
    public class UIInteractionListPanel : MonoBehaviour, IDataView<IEnumerable<IInteraction>, TargetingInfo>
    {
        private MonoPool<UIInteractionButton> _viewPool;
        [SerializeField][NonNull] private LayoutGroup _layout;
        private readonly List<UIInteractionButton> _views = new List<UIInteractionButton>();
        private GameStateContext _gameStateContext;
        private PlayerInputHandler _player;

        public event Action<InteractionCommand> OnPreviewCommand;

        private void Start()
        {
            var prototype = _layout.GetComponentInChildren<UIInteractionButton>();
            _viewPool = new MonoPool<UIInteractionButton>(prototype, 9,9);
            for (int i = 0; i < 8; i++)
            {
                if (_viewPool.TryGet(out var buttonView))
                {
                    _views.Add(buttonView);
                    buttonView.Clear();
                }
            }
            ContextService.Get<GameStateContext>(HandleGameStateContext);
        }
        
        private void HandleGameStateContext(GameStateContext gameStateContext)
        {
            _gameStateContext = gameStateContext;
            _gameStateContext.GetPlayer(player => _player = player);
        }
        
        public void Clear()
        {
            if (_views != null)
            {
                foreach (var buttonView in _views)
                {
                    buttonView.OnPreviewChange -= HandleButtonPreview;
                    buttonView.Clear();
                }
            }
        }

        public void Set(IEnumerable<IInteraction> data, TargetingInfo targetingInfo)
        {
            Clear();
            List<IInteraction> interactions = data.ToList();
            for (int i = 0; i < _views.Count; i++)
            {
                UIInteractionButton buttonView = _views[i];
                buttonView.OnPreviewChange -= HandleButtonPreview;
                if (i < interactions.Count)
                {
                    if (buttonView != null)
                    {
                        buttonView.Set(interactions[i], targetingInfo);
                        buttonView.OnPreviewChange += HandleButtonPreview;
                    }
                }
                else
                {
                    buttonView.Set(null, targetingInfo);
                }
                
            }
        }

        private void HandleButtonPreview(InteractionCommand command)
        {
            OnPreviewCommand?.Invoke(command);
        }
    }
}