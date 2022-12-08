using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Trait;
using NiftyFramework.UI;
using NiftyFramework.UnityUtils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cards
{
    public class UICardSpreadView : MonoBehaviour, IView<List<TraitData>, PlayerData>
    {
        [SerializeField] private LayoutGroup[] _cardLayouts;
        private MonoPool<UICardTraitView> _cardPool;
        private HashSet<UICardTraitView> _views;
        public event Action<Dictionary<TraitData, Guess>> OnTraitGuessesChanged;
        private Dictionary<TraitData, Guess> _guessInfo = new Dictionary<TraitData, Guess>();
        public void Set(List<TraitData> traitDataList, PlayerData playerData)
        {
            List<UICardTraitView> cardViews = new List<UICardTraitView>();
            foreach (var layout in _cardLayouts)
            {
                var cardViewComponents = layout.GetComponentsInChildren<UICardTraitView>();
                cardViews.AddRange(cardViewComponents);
            }

            _views = new HashSet<UICardTraitView>();
            _cardPool = new MonoPool<UICardTraitView>(cardViews);

            int traitIndex = 0;
            for (int row = 0; row < _cardLayouts.Length; row++)
            {
                Transform rowTransform = _cardLayouts[row].transform;
                int len = rowTransform.childCount;

                for (int i = 0; i < len && traitIndex < traitDataList.Count; i++)
                {
                    var trait = traitDataList[traitIndex];
                    if (_cardPool.TryGet(out var view))
                    {
                        if (playerData.TryGetReactionAbilities(trait, out var reactionList, out var abilityList))
                        {
                            view.Set(trait, reactionList.ToList(), abilityList.ToList());
                        }
                        else
                        {
                            view.Set(trait, null, null);
                        }
                    }

                    view.transform.parent = rowTransform;
                    //view.OnToggled += HandleCardToggled;
                    view.OnGuessChange += HandleGuessChanged;
                    _views.Add(view);

                    traitIndex++;
                }
            }
        }

        private void HandleGuessChanged(UICardTraitView view, Guess enumGuess)
        {
            if (view.TraitData == null)
            {
                return;
            }
            if (enumGuess == Guess.None)
            {
                _guessInfo.Remove(view.TraitData);
            }
            else if (_guessInfo != null)
            {
                if (_guessInfo.ContainsKey(view.TraitData))
                {
                    _guessInfo[view.TraitData] = enumGuess;
                }
                else
                {
                    _guessInfo.Add(view.TraitData, enumGuess);
                }
            }
            OnTraitGuessesChanged?.Invoke(_guessInfo);
        }

        public void ClearSelection()
        {
            _guessInfo = null;
            WithAll(item =>
            {
                item.SetFacingDown(false);
            });
        }

        public void WithAll(Action<UICardTraitView> func)
        {
            foreach(var item in _views)
            {
                func(item);
            }
        }

        public void Clear()
        {
            foreach (var view in _views)
            {
                view.Clear();
            }
        }
        
        public void SetGuessInfo(Dictionary<TraitData, Guess> guessInfo)
        {
            _guessInfo = guessInfo;
            if (_views != null)
            {
                foreach (var view in _views)
                {
                    if (guessInfo != null && guessInfo.TryGetValue(view.TraitData, out Guess guessValue))
                    {
                        view.SetGuess(guessValue);
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