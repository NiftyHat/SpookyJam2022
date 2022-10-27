using System;
using Spawn;
using UnityEngine;

namespace Data.Location
{
    public class LocationData : ScriptableObject
    {
        [SerializeField] private string _friendlyName;
        [SerializeField] [TextArea] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _spawnSeed = 523523;

        private CharacterSpawnSet _spawnSet;
        public string FriendlyName => _friendlyName;
        public Sprite Icon => _icon;

        protected Action<CharacterSpawnSet> _onSpawnsSet;

        public int SpawnSeed => _spawnSeed;

        public void SetSpawns(CharacterSpawnSet spawnSet)
        {
            _spawnSet = spawnSet;
            _onSpawnsSet?.Invoke(spawnSet);
        }

        public void AsyncGetSpawns(Action<CharacterSpawnSet> OnSet)
        {
            if (_spawnSet != null)
            {
                OnSet(_spawnSet);
            }
            _onSpawnsSet += OnSet;
        }
    }
}