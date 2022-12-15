using UnityEngine;

namespace UI
{
    public class TooltipAbilitySimple : TooltipSimple
    {
        public readonly string Title;
        public readonly int Cost;

        public TooltipAbilitySimple(Sprite sprite, string bodyCopy, string titleCopy, int cost) : base(sprite, bodyCopy)
        {
            Title = titleCopy;
            Cost = cost;
        }
    }
}