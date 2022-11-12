using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private List<TransitionZoneView> _transitionZoneViews;

    private GameStateContext _gameStateService;
    private CharacterSpawnSet _spawnSet;
    private bool _canSpawn = true;
    
    // Start is called before the first frame update
    private void Awake()
    {
        foreach (var item in _transitionZoneViews)
        {
            item.SetLocation(_locationData);
        }
        if (_locationData != null)
        {
            _locationData.SetInstance(this);
            Set(_locationData);
        }
    }

    void Start()
    {
        ContextService.Get<GameStateContext>(HandleGameState);
    }

    private void HandleGameState(GameStateContext gameStateService)
    {
        _gameStateService = gameStateService;
        Init();
    }

    void SpawnCharacters(IReadOnlyList<CharacterEntity> entities, System.Random random)
    {
        if (_canSpawn && _spawnSet != null)
        {
            _spawnSet.Spawn(entities, random);
            _canSpawn = false;
        }
    }

    private void Init()
    {
        if (_gameStateService.CharacterEntities != null)
        {
            SpawnCharacters(_gameStateService.CharacterEntities, new System.Random(_locationData.SpawnSeed));
        }
    }

    public void Load()
    {
        Init();
    }

    public void Unload()
    {
        gameObject.SetActive(false);
    }

    public void SpawnPlayer(PlayerInputHandler player, LocationData fromLocation)
    {
        Transform playerTransform = player.transform;
        playerTransform.parent = transform;
        TransitionZoneView spawnZone = _transitionZoneViews.FirstOrDefault(item => item.LinkedLocation == fromLocation);
        if (spawnZone == null && _transitionZoneViews.Count > 0)
        {
            spawnZone = _transitionZoneViews[0];
        }
        if (spawnZone != null)
        {
            spawnZone.SpawnPlayer(player);
        }
        else
        {
            playerTransform.position = transform.position;
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

    public bool IsLocation(LocationData location)
    {
        return _locationData = location;
    }

    public void Register(CharacterSpawnSet characterSpawnSet)
    {
        _spawnSet = characterSpawnSet;
        if (_gameStateService != null)
        {
            
        }
    }
}
