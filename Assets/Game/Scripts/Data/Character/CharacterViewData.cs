using UnityEngine;

namespace Data.Character
{
    public class CharacterViewData : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private CharacterReactionAudioData _reactionAudio;
        public Sprite Sprite => _sprite;
        public CharacterReactionAudioData ReactionAudio => _reactionAudio;
    }
}