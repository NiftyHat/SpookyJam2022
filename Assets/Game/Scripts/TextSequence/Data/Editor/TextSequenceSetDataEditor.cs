using Data.TextSequence;
using NiftyScriptableSet;
using TextSequence.Data;
using UnityEditor;

namespace Game.Data.TextSequence
{
    [CustomEditor(typeof(TextSequenceSetData))]
    public class TextSequenceSetDataEditor : ScriptableSetInspector<TextSequenceItem>
    {
        public override string GetEditorListDisplayName(TextSequenceItem item)
        {
            if (item is TextSequenceSetText textSequenceSetText)
            {
                return textSequenceSetText.SummaryText;
            }
            return item.name;
        }
    }
}