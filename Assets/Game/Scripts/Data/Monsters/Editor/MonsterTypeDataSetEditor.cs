using Data.Monsters;
using NiftyScriptableSet;
using UnityEditor;
using UnityEngine;

namespace Game.Data.Monsters
{
    [CustomEditor(typeof(MonsterTypeDataSet))]
    public class MonsterTypeDataSetEditor : ScriptableSetInspector<MonsterEntityTypeData>
    {
    }
}