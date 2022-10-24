using NiftyScriptableSet;
using UnityEngine;

namespace Data.GameOver
{
    [CreateAssetMenu(fileName = "GameOverDataSet", menuName = "Game/GameOverData", order = 3)]
    public class GameOverData : ScriptableSet<GameOverReasonData>
    {

    }
}
