using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class UIWidgetPool: MonoBehaviour
{
    private const int DEFAULT_CAPACITY = 7;
    private const int MAX_POOL_SIZE = 10;

    private static IObjectPool<GameObject> _pool;
    public IObjectPool<GameObject> Pool
    {
        get
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<GameObject>(null, OnTakeFromPool,  OnReturnedToPool, OnDestroyPoolObject, true, DEFAULT_CAPACITY, MAX_POOL_SIZE);
            }
            return _pool;
        }
    }



    // Called when an item is returned to the pool using Release
    void OnReturnedToPool(GameObject go)
    {
        go.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(GameObject go)
    {
        go.SetActive(true);
    }

    void OnDestroyPoolObject(GameObject go)
    {
        Destroy(go);
    }
}

public class IconWidget : MonoBehaviour
{
   
    [SerializeField]
    private Image image;

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
