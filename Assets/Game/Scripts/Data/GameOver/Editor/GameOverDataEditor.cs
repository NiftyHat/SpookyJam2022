using Data.GameOver;
using NiftyScriptableSet;
using UnityEditor;

namespace Game.Data.GameOver
{
    [CustomEditor(typeof(GameOverData))]
    public class GameOverDataEditor : ScriptableSetInspector<GameOverReasonData>
    {
        
    }
}