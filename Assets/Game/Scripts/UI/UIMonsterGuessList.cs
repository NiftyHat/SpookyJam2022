using System;
using System.Collections.Generic;
using Data.Monsters;
using NiftyFramework.DataView;
using NiftyFramework.UnityUtils;
using UnityEngine;

namespace UI
{
    public class UIMonsterGuessList : MonoBehaviour, IDataView<Dictionary<MonsterEntityTypeData, Guess>>
    {
        [SerializeField] private MonsterEntityTypeDataSet _monsterEntityTypeDataSet;
        private MonoPool<UILabeledGuessSelector> _guessPool;

        private Dictionary<MonsterEntityTypeData, UILabeledGuessSelector> _views;
        private Dictionary<MonsterEntityTypeData, Guess> _data;
        public event Action<Dictionary<MonsterEntityTypeData, Guess>> OnChangeSelection;

        public void Start()
        {
            _guessPool = new MonoPool<UILabeledGuessSelector>(GetComponentsInChildren<UILabeledGuessSelector>());
            _views = new Dictionary<MonsterEntityTypeData, UILabeledGuessSelector>();
            foreach (var item in _monsterEntityTypeDataSet.References)
            {
                if (_guessPool.TryGet(out var view))
                {
                    view.Set(item.FriendlyName);
                    _views.Add(item, view);
                    //TODO dsaunders - move this into a static handler.
                    view.OnValueChanged += (Guess value) =>
                    {
                        if (_data != null)
                        {
                            if (_data.ContainsKey(item))
                            {
                                _data[item] = value;
                            }
                            else
                            {
                                _data.Add(item, value);
                            }
                            OnChangeSelection?.Invoke(_data);
                        }
                    };
                }
            }
        }

        public void Clear()
        {
            foreach (var item in _views)
            {
                if (item.Value != null)
                {
                    item.Value.SetGuess(Guess.None);
                }
            }
        }

        public void Set(Dictionary<MonsterEntityTypeData, Guess> data)
        {
            if (data == null)
            {
                Clear();
                return;
            }
            _data = data;
            if (_views != null)
            {
                foreach (var kvp in _views)
                {
                    MonsterEntityTypeData monsterData = kvp.Key;
                    UILabeledGuessSelector view = kvp.Value;
                    if (data.TryGetValue(kvp.Key, out var guess))
                    {
                        view.Set(monsterData.FriendlyName, guess);
                    }
                    else
                    {
                        view.SetGuess(Guess.None);
                    }
                }
            }
        }
    }
}
