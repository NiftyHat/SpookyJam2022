using System;
using System.Collections.Generic;
using Context;
using Data.Location;
using Entity;
using NiftyFramework.Core.Context;
using NiftyFramework.UI;
using Spawn;
using UnityEditor;
using UnityEngine;

public class LocationView : MonoBehaviour, IView<LocationData>
{
    [SerializeField] private LocationData _locationData;

    private GameStateContext _gameStateService;
    private CharacterSpawnSet _spawnSet;
    private bool _canSpawn = true;
    
    // Start is called before the first frame update
    void Start()
    {
        ContextService.Get<GameStateContext>(HandleGameState);
        if (_locationData != null)
        {
            Set(_locationData);
        }
    }

    private void HandleGameState(GameStateContext gameStateService)
    {
        _gameStateService = gameStateService;
        if (_gameStateService.CharacterEntities != null)
        {
            SpawnCharacters(_gameStateService.CharacterEntities, new System.Random(_locationData.SpawnSeed));
        }
    }

    void SpawnCharacters(IReadOnlyList<CharacterEntity> entities, System.Random random)
    {
        if (_canSpawn)
        {
            _spawnSet.Spawn(entities, random);
            _canSpawn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(LocationData viewData)
    {
        _locationData = viewData;
        _locationData.AsyncGetSpawns(HandleSpawns);
    }

    public void OnDrawGizmos()
    {
        if (_locationData != null)
        {
            Handles.Label(transform.position, _locationData.FriendlyName);
        }
    }

    private void HandleSpawns(CharacterSpawnSet spawnSet)
    {
        
    }

    public void Clear()
    {
        //intentionally empty
    }

    public void Register(CharacterSpawnSet characterSpawnSet)
    {
        _spawnSet = characterSpawnSet;
        if (_gameStateService != null)
        {
            
        }
    }
}
