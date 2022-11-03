using System;
using System.Collections.Generic;
using Context;
using Interactions;
using Interactions.Commands;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
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
        
        public event Action<InteractionCommand> OnPreviewCommand;

        private void Start()
        {
            var prototype = _layout.GetComponentInChildren<UIInteractionButton>();
            _viewPool = new MonoPool<UIInteractionButton>(prototype);
            Clear();
        }
        
        public void Clear()
        {
            if (_views != null)
            {
                foreach (var buttonView in _views)
                {
                    buttonView.OnPreviewChange -= HandleButtonPreview;
                    _viewPool.TryReturn(buttonView);
                }
            }
        }

        public void Set(IEnumerable<IInteraction> data, TargetingInfo targetingInfo)
        {
            Clear();
            int index = 0;
            foreach (var item in data)
            {
                if (_viewPool.TryGet(out var buttonView))
                {
                    buttonView.transform.SetSiblingIndex(index);
                    buttonView.OnPreviewChange += HandleButtonPreview;
                    buttonView.Set(item, targetingInfo);
                    _views.Add(buttonView);
                    index++;
                }
            }
        }

        private void HandleButtonPreview(InteractionCommand command)
        {
            OnPreviewCommand?.Invoke(command);
        }
    }
}