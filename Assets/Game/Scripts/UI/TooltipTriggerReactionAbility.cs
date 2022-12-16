using System.Collections.Generic;
using Data.Trait;
using UnityEngine;

namespace UI
{
    public class TooltipTriggerReactionAbility : TooltipAbilitySimple
    {
        public readonly IReadOnlyList<TraitData> Traits;

        public TooltipTriggerReactionAbility(Sprite sprite, string bodyCopy, string titleCopy, int cost, IReadOnlyList<TraitData> traits) : base(sprite, bodyCopy, titleCopy, cost)
        {
            Traits = traits;
        }
    }
}