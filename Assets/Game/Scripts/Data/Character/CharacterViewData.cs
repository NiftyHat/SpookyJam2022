using UnityEngine;

namespace Data.Character
{
    public class CharacterViewData : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        public Sprite Sprite => _sprite;
    }
}