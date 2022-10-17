using System.Collections.Generic;
using GameStats;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace UI.Widgets
{
    public class GameStatSegmentedBarWidget : MonoBehaviour, IView<GameStat>
    {
        [SerializeField][NonNull] private HorizontalOrVerticalLayoutGroup _layoutGroup;

        private List<ImageToggleWidget> _items;
        private MonoPool<ImageToggleWidget> _monoPool;
        private GameStat _gameStat;

        public void Start()
        {
            var prototype = _layoutGroup.GetComponentInChildren<ImageToggleWidget>();
            if (prototype != null)
            {
                _monoPool = new MonoPool<ImageToggleWidget>(prototype);
            }
        }

        public void Set(GameStat gameStat)
        {
            Clear();
            gameObject.SetActive(true);
            _gameStat = gameStat;
            _items = new List<ImageToggleWidget>(gameStat.Max);
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