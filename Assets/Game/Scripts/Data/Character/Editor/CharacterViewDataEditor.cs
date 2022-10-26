using Data.Character;
using Entity;
using NiftyScriptableSet;
using UnityEditor;

namespace Game.Data.Character
{
    [CustomEditor(typeof(CharacterViewDataSet))]
    public class CharacterViewDataEditor : ScriptableSetInspector<CharacterViewData>
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            base.OnInspectorGUI();
        }
    }
}