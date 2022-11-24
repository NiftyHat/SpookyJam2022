using System.Collections.Generic;
using Entity;
using NiftyFramework.Core;
using NiftyFramework.Scripts;
using UnityEngine;

namespace Spawn
{
    public class CharacterSpawnSet : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] [ReadOnly] private LocationView _locationView;
        [SerializeField] [ReadOnly] private List<CharacterSpawnPosition> _spawnPositions;
        private HashSet<CharacterSpawnPosition> _set;

        public void Start()
        {
            _locationView.Register(this);
        }

        public void Spawn(IReadOnlyList<CharacterEntity> entity, System.Random random)
        {
            int[] indexList = ListUtils.GenerateInts(_spawnPositions.Count, random, 0 , _spawnPositions.Count);
            indexList.Shuffle();
            for (int i = 0; i < indexList.Length && i< entity.Count; i++)
            {
                int randomIndex = indexList[i];
                CharacterSpawnPosition spawnPosition = _spawnPositions[randomIndex];
                spawnPosition.Set(entity[i]);
            }
        }

        public void Register(CharacterSpawnPosition spawnPosition)
        {
            _set ??= new HashSet<CharacterSpawnPosition>();
            if (!_set.Contains(spawnPosition))
            {
                if (_spawnPositions != null)
                {
                    _spawnPositions.Add(spawnPosition);
                }
                _set.Add(spawnPosition);
            }
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            _set = new HashSet<CharacterSpawnPosition>(_spawnPositions);
        }
    }
}