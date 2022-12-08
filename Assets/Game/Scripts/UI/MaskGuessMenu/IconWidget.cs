using System.Collections;
using System.Collections.Generic;
using Data;
using NiftyFramework.DataView;
using UnityEngine;
using UnityEngine.UI;

public class IconWidget : MonoBehaviour, IDataView<IIconViewData>
{
   
    [SerializeField]
    private Image image;

    protected Color? _defaultColor;
    private IIconViewData _viewData;

    protected void Awake()
    {
        _defaultColor = image.color;
    }

    public void SetSprite(Sprite sprite)
    {
        image.gameObject.SetActive(true);
        image.sprite = sprite;
    }

    public void SetEnabled(bool isEnabled)
    {
        if (!_defaultColor.HasValue)
        {
            _defaultColor = image.color;
        }
        if (isEnabled)
        {
            if (_viewData != null && _viewData.Tint.TryGet(out var tintColor))
            {
                image.color = tintColor;
            }
            else
            {
                image.color = _defaultColor.Value;
            }
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.2f);
        }
        
    }

    public void Clear()
    {
        image.gameObject.SetActive(false);
        if (_defaultColor.HasValue)
        {
            image.color = _defaultColor.Value;
        }
    }

    public void Set(IIconViewData data)
    {
        image.gameObject.SetActive(true);
        _viewData = data;
        image.sprite = _viewData.Sprite;
        if (_viewData.Tint.TryGet(out var tintColor))
        {
            image.color = tintColor;
        }
        else if (_defaultColor.HasValue)
        {
            image.color = _defaultColor.Value;
        }
    }
}
