using UnityEngine;

namespace Data
{
    public class GameOverReasonData : ScriptableObject
    {
        [SerializeField] [TextArea] protected string _description;

        public string Description => _description;
    }
}
