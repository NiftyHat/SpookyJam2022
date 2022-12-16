using Data.Location;
using Entity;
using NiftyFramework.Core.Utils;
using Spawn;
using TMPro;
using TouchInput.UnitControl;
using UI;
using UnityEngine;
using UnityUtils;

public class TransitionZoneView : InputTargetView
{
    [SerializeField] private LocationData _zoneLocation;
    [SerializeField][NonNull] private LocationData _linkedLocation;
    [SerializeField][NonNull] private LabledTransform _playerSpawnLocation;
    [SerializeField] private CharacterSpawnPosition.FacingDirection _facingDirection;
    [SerializeField][NonNull] private PointerSelectionHandler _pointerSelectionHandler;

    [SerializeField] private LineRenderer _lineOutline;
    [SerializeField] private TextMeshPro _labelTargetLocation;
    [SerializeField] private Sprite _spriteDirectionArrow;

    public LocationData LinkedLocation => _linkedLocation;
    public LocationData ZoneLocation => _zoneLocation;

    public void Start()
    {
        _pointerSelectionHandler.OnSelectChange += HandleSelectionChanged;
        _pointerSelectionHandler.OnOverStateChange += HandleOverChanged;
        _lineOutline?.gameObject.TrySetActive(false);
        if (_linkedLocation != null && _labelTargetLocation != null)
        {
            _labelTargetLocation.SetText(_linkedLocation.FriendlyName);
        }
       
    }

    private void HandleOverChanged(bool isOver)
    {
        if (_lineOutline != null)
        {
            _lineOutline.gameObject.TrySetActive(isOver);
        }
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
        player.SnapCamera();
    }

    public void SetLocation(LocationData locationData)
    {
        _zoneLocation = locationData;
    }

}
