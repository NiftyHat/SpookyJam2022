using System;
using Data.Reactions;
using Data.Trait;
using UnityEngine;

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
        }

        [SerializeField] private Item[] _triggerList;
    }
}