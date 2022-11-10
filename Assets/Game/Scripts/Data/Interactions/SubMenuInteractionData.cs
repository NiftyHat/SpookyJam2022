using System.Collections.Generic;
using Commands;
using Context;
using Interactions;
using Interactions.Commands;
using NiftyFramework.Core.Context;

namespace Data.Interactions
{
    public abstract class SubMenuInteractionData : InteractionData
    {
        private class Command : InteractionCommand
        {
            private List<IInteraction> _menuItems;
            
            public Command(IInteraction interaction, TargetingInfo targets, List<IInteraction> menuItems) : base(interaction, targets)
            {
                _menuItems = menuItems;
            }

            public override void Execute(Completed onDone)
            {
                ContextService.Get<GameStateContext>(gameStateContext =>
                {
                    //gameStateContext.ShowSubMenu(_subCommands);
                });
                
                base.Execute(onDone);
            }

            public override string GetDescription()
            {
                return _interaction.GetDescription();
            }
        }

        protected abstract List<IInteraction> GetSubCommands();

        public override InteractionCommand GetCommand(TargetingInfo targetingInfo)
        {
            return new Command(this, targetingInfo, GetSubCommands());
        }
    }
}