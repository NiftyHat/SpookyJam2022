using System.Collections.Generic;
using Data;
using Data.Trait;
using NiftyFramework.UI;
using NiftyFramework.UnityUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Guide
{
    public class UIGuideTrait : MonoBehaviour, IView<TraitData, PlayerData>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _labelName;
        [SerializeField] private LayoutGroup _abilityReactionPairLayout;

        private List<UIGuideAbilityReactionPair> _abilityReactionPairs;

        private MonoPool<UIGuideAbilityReactionPair> _abilityReactionPairPool;
        private void Awake()
        {
            InitPool();
        }

        private void InitPool()
        {
            if (_abilityReactionPairPool == null)
            {
                UIGuideAbilityReactionPair[] poolItems = _abilityReactionPairLayout.GetComponentsInChildren<UIGuideAbilityReactionPair>();
                _abilityReactionPairPool = new MonoPool<UIGuideAbilityReactionPair>(poolItems);
            }
        }
        
        public void Set(TraitData traitData, PlayerData playerData)
        {
            InitPool();
            _icon.sprite = traitData.Icon;
            _labelName.SetText(traitData.FriendlyName);
            if (playerData != null)
            {
                playerData.TryGetReactionAbilities(traitData, out _, out var abilityList);
                foreach (var ability in abilityList)
                {
                    if (_abilityReactionPairPool.TryGet(out var view))
                    {
                        var reaction = ability.ReactionTrigger.GetReaction(traitData);
                        if (ability != null && reaction != null)
                        {
                            view.Set(reaction, ability);
                        }
                        else
                        {
                            view.Clear();
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}