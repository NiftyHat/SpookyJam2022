using System;
using System.Collections.Generic;
using System.Linq;
using Data.Reactions;
using UnityEngine;

namespace Data.Character
{
    [CreateAssetMenu(fileName = "CharacterReactionAudioData", menuName = "Game/Characters/CharacterReactionAudioData", order = 1)]
    public class CharacterReactionAudioData : ScriptableObject, ISerializationCallbackReceiver
    {
        [Serializable]
        public class Entry
        {
            [SerializeField] private ReactionData _reactionData;
            [SerializeField] private AudioClip[] _audioClips;

            public ReactionData ReactionData => _reactionData;
            public bool TryGetFirst(out AudioClip clip)
            {
                if (_audioClips != null && _audioClips.Length > 0)
                {
                    clip = _audioClips.First();
                    return clip != null;
                }
                clip = null;
                return false;
            }
        }

        [SerializeField] private Entry[] _entryList;
        public Dictionary<ReactionData, Entry> _lookup;

        public bool TryGetClip(ReactionData reactionData, out AudioClip clip)
        {
            if (_lookup != null && _lookup.TryGetValue(reactionData, out var entry))
            {
                return entry.TryGetFirst(out clip);
            }
            clip = null;
            return false;
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            _lookup = new Dictionary<ReactionData, Entry>();
            foreach (var item in _entryList)
            {
                if (item.ReactionData != null)
                {
                    _lookup[item.ReactionData] = item;
                }
            }
        }
    }
}