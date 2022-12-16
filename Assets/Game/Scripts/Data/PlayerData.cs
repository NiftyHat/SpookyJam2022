using System;
using System.Collections.Generic;
using System.Linq;
using Data.Interactions;
using Data.Reactions;
using Data.Trait;
using GameStats;
using NiftyFramework.Core.Utils;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        private GameStat _actionPoints = new GameStat("ActionPoints","AP", 100,100);
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _name;
        [SerializeField,Tooltip("Set this separately because movement interactions have special rules")] 
        private MoveInteractionData _moveInteraction;
        [SerializeField] private List<InteractionData> _abilityInteractionList;
        [SerializeField][NonNull] private TimeData _timeData;

        public MoveInteractionData MoveInteraction => _moveInteraction;
        public List<InteractionData> InteractionList => _abilityInteractionList;

        public GameStat ActionPoints => _actionPoints;

        public List<TData> GetInteractionDataList<TData>(Func<TData, bool> filter = null) where TData : InteractionData
        {
            List<TData> output = new List<TData>();
            foreach (var item in _abilityInteractionList)
            {
                if (item is TData typed)
                {
                    if (filter == null)
                    {
                        output.Add(typed);
                    }
                    else
                    {
                        if (filter(typed))
                        {
                            output.Add(typed);
                        }
                    }
                }
            }
            return output;
        }

        public bool TryGetReactionAbilities(TraitData trait, out HashSet<ReactionData> reactionDataList, out List<AbilityReactionTriggerData> abilityDataList)
        {
            reactionDataList = new HashSet<ReactionData>();
            abilityDataList = new List<AbilityReactionTriggerData>();
            var playerReactionAbilities = GetInteractionDataList<AbilityReactionTriggerData>();
            bool hasResults = false;
            foreach (var ability in playerReactionAbilities)
            {
                if (ability.ReactionTrigger.TryGetReaction(trait, out var abilityReactionTriggers, item => item.isMiss == false))
                {
                    abilityDataList.Add(ability);
                    reactionDataList.UnionWith(abilityReactionTriggers);
                    hasResults = true;
                }
            }
            if (hasResults)
            {
                return true;
            }
            return false;
        }

    }
}