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

    protected Color defaultColor;

    protected void Start()
    {
        defaultColor = image.color;
    }

    public void SetSprite(Sprite sprite)
    {
        image.gameObject.SetActive(true);
        image.sprite = sprite;
    }

    public void Clear()
    {
        image.gameObject.SetActive(false);
        image.color = defaultColor;
    }

    public void Set(IIconViewData data)
    {
        image.gameObject.SetActive(true);
        image.sprite = data.Sprite;
        if (data.Tint.TryGet(out var tintColor))
        {
            image.color = tintColor;
        }
        else
        {
            image.color = defaultColor;
        }
    }
}
