using System.Collections.Generic;
using Interactions;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace UI.Targeting
{
    public class UIInteractionListPanel : MonoBehaviour, IDataView<IEnumerable<IInteraction>>
    {
        private MonoPool<UIInteractionButton> _buttonPool;
        [SerializeField][NonNull] private LayoutGroup _layout;

        private void Start()
        {
            var prototype = _layout.GetComponentInChildren<UIInteractionButton>();
            _buttonPool = new MonoPool<UIInteractionButton>(prototype);
            Clear();
        }

        public void Clear()
        {
            _buttonPool.Clear();
        }

        public void Set(IEnumerable<IInteraction> data)
        {
            _buttonPool.Clear();
            foreach (var item in data)
            {
                if (_buttonPool.TryGet(out var buttonView))
                {
                    buttonView.Set(item);
                }
            }
        }
    }
}