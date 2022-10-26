using System;
using System.Collections.Generic;
using Spawn;
using UnityEngine;

namespace Data.Area
{
    public class AreaData : ScriptableObject
    {
        [SerializeField] private string _friendlyName;
        [SerializeField] [TextArea] private string _description;
        [SerializeField] private Sprite _icon;

        private CharacterSpawnSet _spawnSet;
        public string FriendlyName => _friendlyName;
        public Sprite Icon => _icon;

        protected Action<CharacterSpawnSet> _onSpawnsSet;

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