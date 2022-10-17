using System.Collections;
using System.Collections.Generic;
using Context;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButtonWidget : MonoBehaviour
{
    [SerializeField] [NonNull] private Button _button;
    [SerializeField] [NonNull] private TextMeshProUGUI _label;
    private GameStateContext _gameStateContext;

    // Start is called before the first frame update
    void Start()
    {
        ContextService.Get<GameStateContext>(HandleGameStateContext);
        _button.onClick.AddListener(HandleButtonClicked);
    }

    private void HandleButtonClicked()
    {
        _gameStateContext.NextTurn();
    }

    private void HandleGameStateContext(GameStateContext service)
    {
        _gameStateContext = service;
        _gameStateContext.OnTurnStarted += HandleTurnStarted;
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
    }

    private void HandleEndTurnClicked()
    {
        _gameStateContext?.NextTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
