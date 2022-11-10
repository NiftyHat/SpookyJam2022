using Data.Location;
using Entity;
using NiftyFramework.Core.Utils;
using Spawn;
using TouchInput.UnitControl;
using UI;
using UnityEngine;

public class TransitionZoneView : InputTargetView
{
    [SerializeField][NonNull] private LocationData _linkedLocation;
    [SerializeField][NonNull] private LabledTransform _playerSpawnLocation;
    [SerializeField] private CharacterSpawnPosition.FacingDirection _facingDirection;
    [SerializeField][NonNull] private PointerSelectionHandler _pointerSelectionHandler;

    public LocationData LinkedLocation => _linkedLocation;

    public void Start()
    {
        _pointerSelectionHandler.OnSelectChange += HandleSelectionChanged;
    }

    private void HandleSelectionChanged(bool isSelected)
    {
        //TODO show selected.
    }

    public Vector3 GetPlayerSpawnLocation()
    {
        if (_playerSpawnLocation != null)
        {
            return _playerSpawnLocation.transform.position;
        }
        return transform.position;
    }

    public void SpawnPlayer(PlayerInputHandler player)
    {
        Transform playerTransform = player.transform;
        playerTransform.position = GetPlayerSpawnLocation();
        if (player.TryGetComponent<FacingDirectionView>(out var facingDirection))
        {
            facingDirection.Set(_facingDirection);
        }
    }

    public void DespawnPlayer(PlayerInputHandler player)
    {
        player.gameObject.SetActive(false);
        player.transform.parent = null;
    }
    
}
