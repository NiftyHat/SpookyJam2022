using NiftyFramework;
using NiftyFramework.UI;
using TMPro;
using UnityEngine;

public class UICardHeaderView : MonoBehaviour, IView<string, int>, IView<string>
{

    [SerializeField] private TextMeshProUGUI _textHeader;
    [SerializeField] private TextMeshProUGUI _textSuit;
    [SerializeField] private TextMeshProUGUI _textValue;

    public void Set(string copy, int value)
    {
        _textHeader.gameObject.SetActive(false);
        _textSuit.gameObject.SetActive(true);
        _textValue.gameObject.SetActive(true);
        _textSuit.SetText(copy);
        _textValue.SetText(ToRoman.ToRomanNumeral(value));
    }

    public void Set(string copy)
    {
        _textHeader.gameObject.SetActive(true);
        _textSuit.gameObject.SetActive(false);
        _textValue.gameObject.SetActive(false);
        _textHeader.SetText(copy);
    }

    public void Clear()
    {
        _textHeader.gameObject.SetActive(false);
        _textSuit.gameObject.SetActive(false);
        _textValue.gameObject.SetActive(false);
    }
}
