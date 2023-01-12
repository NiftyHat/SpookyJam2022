using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    private Action _onDeferedSpawn;
    
    private void Awake()
    {
        foreach (var item in _transitionZoneViews)
        {
            if (item != null)
            {
                item.SetLocation(_locationData);
            }
        }
        if (_locationData != null)
        {
            _locationData.SetInstance(this);
            Set(_locationData);
        }
    }

    void Start()
    {
        Init();
    }

    private void HandleGameState(GameStateContext gameStateService)
    {
        _gameStateService = gameStateService;
        
    }

    private void HandlePhaseChange(int newvalue, int oldvalue)
    {
        _canSpawn = true;
        var charactersInLocation = _gameStateService.GetCharactersInLocation(_locationData);
        if (charactersInLocation != null && charactersInLocation.Count > 0)
        {
            SpawnCharacters(charactersInLocation, new System.Random(_locationData.SpawnSeed));
        }
    }

    void SpawnCharacters(IReadOnlyList<CharacterEntity> entities, System.Random random)
    {
        if (_canSpawn)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in entities)
            {
                sb.Append(item.Name.Full);
                sb.Append(",");
            }
            Debug.Log($"{_locationData.FriendlyName} = {sb}");
            if (_spawnSet != null)
            {
                _spawnSet.Spawn(entities, random);
                _canSpawn = false;
            }
            else
            {
                _onDeferedSpawn = () => SpawnCharacters(entities, random);
            }
        }
    }

    protected void Init()
    {
        if (_gameStateService == null)
        {
            ContextService.Get<GameStateContext>(gameStateService =>
            {
                if (_gameStateService == null)
                {
                    _gameStateService = gameStateService;
                    gameStateService.OnPhaseChange += HandlePhaseChange;
                    Init();
                }
            });
            return;
        }
        var charactersInLocation = _gameStateService.GetCharactersInLocation(_locationData);
        if (charactersInLocation != null && charactersInLocation.Count > 0)
        {
            SpawnCharacters(charactersInLocation, new System.Random(_locationData.SpawnSeed));
        }
    }

    public void Load()
    {
        Init();
    }

    public void Unload()
    {
        _canSpawn = true;
        gameObject.SetActive(false);
    }

    public void SpawnPlayer(PlayerInputHandler player, LocationData fromLocation)
    {
        _canSpawn = true;
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
#if UNITY_EDITOR
            Handles.Label(transform.position, _locationData.FriendlyName);
            #endif
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
        if (_onDeferedSpawn != null)
        {
            _onDeferedSpawn.Invoke();
            _onDeferedSpawn = null;
        }
    }
}
