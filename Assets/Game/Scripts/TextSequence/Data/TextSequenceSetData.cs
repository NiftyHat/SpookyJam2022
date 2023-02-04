using NiftyScriptableSet;
using UnityEngine;

namespace Data.TextSequence
{
    [CreateAssetMenu(fileName = "TextSequenceSetData", menuName = "Game/TextSequence/TextSequenceSetData", order = 1)]
    public class TextSequenceSetData : ScriptableSet<TextSequenceItem>
    {
        
    }
}