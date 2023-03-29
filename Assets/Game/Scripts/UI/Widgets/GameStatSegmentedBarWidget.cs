using System.Collections.Generic;
using GameStats;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using NiftyFramework.UnityUtils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class GameStatSegmentedBarWidget : MonoBehaviour, IView<GameStat>
    {
        [SerializeField][NonNull] private HorizontalOrVerticalLayoutGroup _layoutGroup;

        private List<ImageActiveToggleWidget> _items;
        private MonoPool<ImageActiveToggleWidget> _monoPool;
        private GameStat _gameStat;

        public void Start()
        {
            var prototype = _layoutGroup.GetComponentInChildren<ImageActiveToggleWidget>();
            if (prototype != null)
            {
                _monoPool = new MonoPool<ImageActiveToggleWidget>(prototype);
            }
        }

        public void Set(GameStat gameStat)
        {
            Clear();
            gameObject.SetActive(true);
            _gameStat = gameStat;
            _items = new List<ImageActiveToggleWidget>(gameStat.Max);
            for (int i = 0; i < gameStat.Max; i++)
            {
                _monoPool.TryGet(out var instance);
                if (i <= gameStat.Value)
                {
                    instance.Set(true);
                }
                else
                {
                    instance.Set(false);
                }
                instance.transform.parent = _layoutGroup.transform;
                instance.transform.SetSiblingIndex(i);
            }

            gameStat.OnChanged += HandleStatChanged;
        }

        private void HandleStatChanged(int oldValue, int newValue)
        {
            if (newValue > oldValue)
            {
                for (int i = oldValue; i <= newValue; i++)
                {
                    var widget = _items[i];
                    widget.Set(i <= newValue);
                }
            }
            else
            {
                for (int i = newValue; i <= oldValue; i++)
                {
                    var widget = _items[i];
                    widget.Set(i <= newValue);
                }
            }
        }

        public void Clear()
        {
            if (_gameStat != null)
            {
                _gameStat.OnChanged -= HandleStatChanged;
            }
            if (_items != null)
            {
                foreach (var item in _items)
                {
                    _monoPool.TryReturn(item);
                }
                _items.Clear();
            }
            gameObject.SetActive(false);
        }
    }
}