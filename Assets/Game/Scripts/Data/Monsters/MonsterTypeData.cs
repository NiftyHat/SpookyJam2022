using Entity;
using UnityEngine;

namespace Data.Monsters
{
    
    public class MonsterTypeData : ScriptableObject, IRevealProvider
    {
        [SerializeField] private string _friendlyName;
        [SerializeField] private Sprite _revealSprite;

        public Sprite RevealSprite => _revealSprite;
        public string FriendlyName => _friendlyName;
    }
}