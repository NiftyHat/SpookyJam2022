using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Reactions;
using Data.Trait;
using FluentCsv;
using NiftyFramework.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Data.Interactions
{
    [Serializable]
    public class ReactionTriggerSet
    {
        [Serializable]
        public struct Item
        {
            [SerializeField] private TraitData _trait;
            [SerializeField] private ReactionData _reaction;

            public TraitData Trait => _trait;
            public ReactionData Reaction => _reaction;
        }

        [SerializeField] private Item[] _triggerList;
        [SerializeField] private List<ReactionData> _failList;

        public StringBuilder GetTraitList()
        {
            StringBuilder builder = new StringBuilder();
            string separator = "";
            _triggerList.ForEach(
                val =>
                {
                    builder.Append(separator).Append(val.Trait.FriendlyName);
                    separator = ",";
                });
            return builder;
        }

        public ReactionData GetFail()
        {
            return _failList.RandomItem();
        }

        public List<ReactionData> TryGetReaction(HashSet<TraitData> targetTraits)
        {
            HashSet<ReactionData> reactionList = new HashSet<ReactionData>();
            foreach (var trigger in _triggerList)
            {
                if (targetTraits.Contains(trigger.Trait))
                {
                    reactionList.Add(trigger.Reaction);
                }
            }
            return reactionList.ToList();
        }
    }
}