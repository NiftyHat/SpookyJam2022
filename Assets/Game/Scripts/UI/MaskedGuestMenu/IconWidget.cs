using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class IconWidget : MonoBehaviour
{
   
    [SerializeField]
    private Image image;

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
