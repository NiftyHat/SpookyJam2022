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
        public event Action<List<TraitData>> OnSelectedTraitsChanged;
        private List<TraitData> _selected = new List<TraitData>();
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
                    view.OnToggled += HandleCardToggled;
                    _views.Add(view);

                    traitIndex++;
                }
            }
        }

        public void ClearSelection()
        {
            _selected.Clear();
            WithAll(item =>
            {
                item.SetFacingDown(false);
            });
        }

        private void HandleCardToggled(UICardTraitView cardView, bool isToggled)
        {
            if (isToggled)
            {
                if (!_selected.Contains(cardView.TraitData))
                {
                    _selected.Add(cardView.TraitData);
                }
            }
            else
            {
                _selected.Remove(cardView.TraitData);
            }
            OnSelectedTraitsChanged?.Invoke(_selected);
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
        
        public void SetSelected(List<TraitData> currentCharacterTraitGuessList)
        {
            _selected = new List<TraitData>(currentCharacterTraitGuessList);
            foreach (var view in _views)
            {
                if (view.TraitData != null)
                {
                    view.SetTabbedOut(_selected.Contains(view.TraitData));
                }
                else
                {
                    view.SetTabbedOut(false);
                }
            }
        }
    }
}