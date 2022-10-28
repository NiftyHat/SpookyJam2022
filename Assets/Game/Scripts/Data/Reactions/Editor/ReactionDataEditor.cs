using Data.Reactions;
using NiftyScriptableSet;
using UnityEditor;

namespace Game.Data.Reactions
{
    [CustomEditor(typeof(ReactionDataSet))]
    public class ReactionDataEditor : ScriptableSetInspector<ReactionData>
    {
    }
}
