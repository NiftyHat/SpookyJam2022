using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameStats;
using NiftyFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

public class UIGameStatBar : MonoBehaviour, IView<GameStat>
{

    [SerializeField] private Image _fill;
    [SerializeField] private TextMeshProUGUI _label;

    protected GameStat _gameStat;
    protected Tweener _fillTween;

    public void Set(GameStat gameStat)
    {
        bool isFirstInit = false;
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            isFirstInit = true;
        }
        if (_gameStat != gameStat)
        {
            isFirstInit = true;
            if (_gameStat != null)
            {
                _gameStat.OnChanged -= HandleStatChanged;
            }
            _gameStat = gameStat;
            if (gameStat != null)
            {
                gameStat.OnChanged += HandleStatChanged;
            }
        }
        if (isFirstInit && _gameStat != null)
        {
            SetFill(_gameStat.Percentage(), 0);
            SetLabel(_gameStat.Value);
        }
    }

    protected void SetFill(float amount, float animTime = 0f)
    {
        if (animTime > 0)
        {
            _fillTween = _fill.transform.DOScaleX(amount, animTime);
        }
        else
        {
            if (_fillTween != null)
            {
                _fillTween.Kill();
            }
            Transform fillTransform = _fill.transform;
            Vector3 fillScale = fillTransform.localScale;
            fillTransform.localScale = new Vector3(amount, fillScale.y, fillScale.z);
        }
    }

    protected void SetLabel(int amount)
    {
        _label.SetText($"{amount}{_gameStat.Abbreviation}");
    }

    private void HandleStatChanged(int newValue, int oldValue)
    {
        if (_gameStat == null)
        {
            SetFill(0);
            _label.SetText("");
        }

        if (_gameStat.Max == 0)
        {
            SetFill(0);
            SetLabel(0);
        }
        SetFill(_gameStat.Percentage(), 0.2f);
        SetLabel(newValue);
    }

    public void Clear()
    {
        SetFill(0);
        _label.SetText("");
        gameObject.TrySetActive(false);
    }
}
