using Febucci.UI.Core;

namespace NiftyFramework.TextAnimatorExtensions
{
    [Febucci.UI.Core.EffectInfo(tag: "typewriter")]
    [UnityEngine.Scripting.Preserve] 
    public class TypewriterSpeedBehaviour : AppearanceBase //<--- for appearance effects and disappearances
    {
        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            throw new System.NotImplementedException();
        }

        public override void SetDefaultValues(AppearanceDefaultValues data)
        {
           // data.defaults.
        }
    }
}