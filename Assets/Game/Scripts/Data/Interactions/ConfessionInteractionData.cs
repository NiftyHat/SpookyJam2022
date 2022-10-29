using Entity;
using UnityEngine;

namespace Data.Interactions
{
    public class ConfessionInteractionData : TargetedInteractionData<CharacterView>
    {
        public override float GetMaxRange()
        {
            return Range;
        }

        public override bool ConfirmInput(RaycastHit hitInfo)
        {
            if (base.ConfirmInput(hitInfo) && Target.Entity != null)
            {
                if (Target.Entity is MonsterEntity)
                {
                    
                }
            }
            return false;
        }
    }
}