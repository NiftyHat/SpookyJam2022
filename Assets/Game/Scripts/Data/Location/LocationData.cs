using System;
using System.Linq;
using NiftyFramework.Core.Utils;
using Spawn;
using UI.Audio;
using UnityEngine;
using UnityUtils;

namespace Data.Location
{
    public class LocationData : ScriptableObject
    {
        [SerializeField] private string _friendlyName;
        [SerializeField] [TextArea] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _spawnSeed = 523523;
        [SerializeField][NonNull] private LocationView _prefab;
        [SerializeField] private AmbientAudioPlayer.AudioState _audioState;

        private LocationView _instance;
        public LocationView Instance => _instance;

        private CharacterSpawnSet _spawnSet;
        public string FriendlyName => _friendlyName;
        public Sprite Icon => _icon;

        protected Action<CharacterSpawnSet> _onSpawnsSet;

        public int SpawnSeed => _spawnSeed;
        public AmbientAudioPlayer.AudioState AudioState => _audioState;

        public void SetSpawns(CharacterSpawnSet spawnSet)
        {
            _spawnSet = spawnSet;
            _onSpawnsSet?.Invoke(spawnSet);
        }

        public LocationView GetInstance()
        {
            if (_instance == null)
            {
                _instance = Instantiate(_prefab);
            }
            else
            {
                _instance.TrySetActive(true);
            }
            return _instance;
        }

        public void AsyncGetSpawns(Action<CharacterSpawnSet> OnSet)
        {
            if (_spawnSet != null)
            {
                OnSet(_spawnSet);
            }
            _onSpawnsSet += OnSet;
        }

        public void SetInstance(LocationView locationView)
        {
            if (_instance == null)
            {
                _instance = locationView;
            }
        }
    }
}