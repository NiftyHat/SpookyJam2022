using System.Collections;
using System.Collections.Generic;
using Context;
using Entity;
using GameStats;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

public class EndTurnButtonWidget : MonoBehaviour
{
    [SerializeField] [NonNull] private Button _button;
    [SerializeField] [NonNull] private TextMeshProUGUI _label;
    [SerializeField] [NonNull] private TextMeshProUGUI _turnLabel;
    [SerializeField] [NonNull] private Image _glow;
    [SerializeField] private int _glowAPThreshold;
    private GameStateContext _gameStateContext;
    private GameStat _turns;

    // Start is called before the first frame update
    void Start()
    {
        ContextService.Get<GameStateContext>(HandleGameStateContext);
        _button.onClick.AddListener(HandleButtonClicked);
        _glow.TrySetActive(false);
    }

    private void HandleButtonClicked()
    {
        _gameStateContext.NextTurn();
    }

    private void HandleGameStateContext(GameStateContext service)
    {
        _gameStateContext = service;
        _turns = _gameStateContext.Turns;
        _gameStateContext.OnTurnStarted += HandleTurnStarted;
        if (_turnLabel != null)
        {
            int turnsRemaining = _turns.Max - _turns.Value;
            _turnLabel.SetText(turnsRemaining.ToString());
        }
        _gameStateContext.GetPlayer(HandlePlayer);
    }

    private void HandlePlayer(PlayerInputHandler player)
    {
        if (player != null)
        {
            player.ActionPoints.OnChanged += HandleActionPointsChanged;
        }
    }

    private void HandleActionPointsChanged(int newValue, int oldValue)
    {
        if (newValue < _glowAPThreshold)
        {
            _glow.TrySetActive(true);
        }
        else
        {
            _glow.TrySetActive(false);
        }
    }

    private void HandleTurnStarted(int turn, int turnMax, int phase)
    {
        if (turn < turnMax)
        {
            _label.SetText("End Turn");
        }
        else
        {
            _label.SetText("Give Up");
        }

        if (_turnLabel != null)
        {
            _turnLabel.SetText((turnMax - turn).ToString());
        }
    }
}
