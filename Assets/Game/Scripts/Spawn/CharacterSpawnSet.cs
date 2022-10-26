using System.Collections.Generic;
using Data.Area;
using Entity;
using NiftyFramework.Core;
using NiftyFramework.Scripts;
using UnityEngine;

namespace Spawn
{
    public class CharacterSpawnSet : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private AreaData _areaData;
        [SerializeField] [ReadOnly] private List<CharacterSpawnLocation> _locations;
        private HashSet<CharacterSpawnLocation> _set;

        public void Start()
        {
            _areaData.SetSpawns(this);
        }

        public void Spawn(CharacterEntity entity, System.Random random)
        {
            var randomSpawn = _locations.RandomItem(random);
            randomSpawn.Set(entity);
        }

        public void Register(CharacterSpawnLocation spawnLocation)
        {
            _set ??= new HashSet<CharacterSpawnLocation>();
            if (!_set.Contains(spawnLocation))
            {
                _locations.Add(spawnLocation);
                _set.Add(spawnLocation);
            }
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            _set = new HashSet<CharacterSpawnLocation>(_locations);
        }
    }
}