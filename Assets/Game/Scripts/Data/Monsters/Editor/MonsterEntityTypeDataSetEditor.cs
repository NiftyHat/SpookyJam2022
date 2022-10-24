using Data.Monsters;
using NiftyScriptableSet;
using UnityEditor;

namespace Game.Data.Monsters
{
    [CustomEditor(typeof(MonsterEntityTypeDataSet))]
    public class MonsterEntityTypeDataSetEditor : ScriptableSetInspector<MonsterEntityTypeData>
    {
    }
}