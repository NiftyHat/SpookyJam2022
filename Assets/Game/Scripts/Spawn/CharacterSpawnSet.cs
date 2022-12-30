using System.Collections.Generic;
using Entity;
using NiftyFramework.Core;
using NiftyFramework.Scripts;
using UnityEngine;
using UnityUtils;

namespace Spawn
{
    public class CharacterSpawnSet : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] [ReadOnly] private LocationView _locationView;
        [SerializeField] [ReadOnly] private List<CharacterSpawnPosition> _spawnPositions;
        private HashSet<CharacterSpawnPosition> _set;

        public void Awake()
        {
            _locationView.Register(this);
        }

        public void Spawn(IReadOnlyList<CharacterEntity> entityList, System.Random random)
        {
            var possibleSpawns = new List<CharacterSpawnPosition>() { };
            possibleSpawns.AddRange(_spawnPositions);
            possibleSpawns.Shuffle(random);
            
            int[] indexList = ListUtils.GenerateInts(_spawnPositions.Count, random, 0 , _spawnPositions.Count);
            indexList.Shuffle();
            
            Pair<CharacterEntity> followPair = default;
            CharacterSpawnPosition spawnReservedForFollowing = null;
            for (int i = 0; i < indexList.Length; i++)
            {
                int randomIndex = indexList[i];
                CharacterSpawnPosition spawnPosition = _spawnPositions[randomIndex];
                CharacterEntity entity = (i < entityList.Count) ? entityList[i] : null;
                if (entity != null)
                {
                    if (entity.FollowTarget != null)
                    {
                        followPair = new Pair<CharacterEntity>(entity, entity.FollowTarget);
                        spawnReservedForFollowing = GetClosest(spawnPosition);
                    }
                    if (entity == followPair.Right && spawnReservedForFollowing != null)
                    {
                        continue;
                    }
                    spawnPosition.Set(entity);
                }
                else
                {
                    spawnPosition.Clear();
                }
                
                
                
            }
            if (followPair.Right != null && spawnReservedForFollowing != null)
            {
                if (spawnReservedForFollowing.TrySwap(followPair.Right, out var swappedCharacter))
                {
                    int unoccupiedIndex = indexList[ entityList.Count];
                    var swapToLocation = _spawnPositions[unoccupiedIndex];
                    swapToLocation.Set(swappedCharacter);
                }
            }
        }

        public CharacterSpawnPosition GetClosest(CharacterSpawnPosition spawnPosition)
        {
            float shortestDistance = float.MaxValue;
            CharacterSpawnPosition best = null;
            for (int i = 0; i < _spawnPositions.Count; i++)
            {
                var item = _spawnPositions[i];
                if (item == spawnPosition)
                {
                    continue;
                }
                float distance = Vector3.Distance(spawnPosition.gameObject.transform.position, item.gameObject.transform.position);
                if (distance < shortestDistance)
                {
                    best = item;
                    shortestDistance = distance;
                }
            }
            return best;
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