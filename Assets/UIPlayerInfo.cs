using System;
using Context;
using Entity;
using GameStats;
using NiftyFramework.Core.Context;
using NiftyFramework.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour, IView<PlayerInputHandler>
{
    [SerializeField] private Button _buttonSelect;
    [SerializeField] private UIGameStatBar _apBar;

    public Action OnSelectPlayer;

    public void Start()
    {
        ContextService.Get<GameStateContext>(HandleGameState);
        _buttonSelect.onClick.AddListener(HandleSelectClicked);
    }

    private void HandleSelectClicked()
    {
        OnSelectPlayer?.Invoke();
    }

    private void HandleGameState(GameStateContext service)
    {
        service.GetPlayer(Set);
    }

    public void Set(PlayerInputHandler player)
    {
        gameObject.SetActive(true);
        _apBar.Set(player.ActionPoints);
    }

    public void Clear()
    {
        gameObject.SetActive(false);
        _apBar.Clear();
    }
}
