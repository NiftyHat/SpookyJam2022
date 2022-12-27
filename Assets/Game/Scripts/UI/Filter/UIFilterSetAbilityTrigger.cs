using Data.Interactions;

namespace UI.Filter
{
    public class UIFilterSetAbilityTrigger : UIFilterButtonSetView<AbilityReactionTriggerData>
    {
        public void Start()
        {
            SetLabel(_defaultTitle);
        }
    }
}