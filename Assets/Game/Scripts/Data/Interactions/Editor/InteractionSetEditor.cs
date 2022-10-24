using Data.Interactions;
using Interactions;
using NiftyScriptableSet;
using UnityEditor;

namespace Game.Data.Interactions
{
    [CustomEditor(typeof(InteractionSet))]
    public class InteractionSetEditor  : ScriptableSetInspector<InteractionData>
    {
        
    }

}