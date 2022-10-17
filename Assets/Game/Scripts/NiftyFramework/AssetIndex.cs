using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NiftyFramework
{
    [CreateAssetMenu]
    public class AssetIndex : ScriptableObject, ISerializationCallbackReceiver, IAssetIndex
    {
        [SerializeField] private string _directory;
        [SerializeField] private ScriptableObject[] _references;
        public ReadOnlyDictionary<Type, IList<ScriptableObject>> Map { get; private set; }
        public string Directory => _directory;

        public void OnBeforeSerialize()
        {
            //UpdateReferences();
        }

        public void OnEnable()
        {
            UpdateReferences();
        }

        public bool TryGet<TAsset>(out TAsset asset)
        {
            System.Type key = typeof(TAsset);
            if (Map.TryGetValue(key, out IList<ScriptableObject> storedItems))
            {
                var first = storedItems.First();
                if (first is TAsset storedAsset)
                {
                    asset = storedAsset;
                    return true;
                }
                else
                {
                    Debug.LogWarning($"Asset {first} stored at {key} cannot cast to given type {typeof(TAsset)}");
                }
            }
            asset = default;
            return false;
        }
        
        public bool TryGet<TAsset>(out IList<TAsset> assetList) where TAsset : ScriptableObject
        {
            Type key = typeof(TAsset);
            if (Map.TryGetValue(key, out IList<ScriptableObject> storedItems))
            {
                IList<ScriptableObject> list = storedItems;
                var castedList = list.Cast<TAsset>();
                assetList = castedList.ToList();
                return true;
            }
            assetList = null;
            return false;
        }

        [ContextMenu("UpdateReferences")]
        public void UpdateReferences()
        {
            List<ScriptableObject> newAssets = new List<ScriptableObject>();
            string assetPath = AssetDatabase.GetAssetPath(this);
            _directory = Path.GetDirectoryName(assetPath);
            string[] assetGuids = AssetDatabase.FindAssets("t:ScriptableObject", new []{_directory});
            foreach (string guid in assetGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.EndsWith(".asset"))
                {
                    var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                    if (asset != null && asset != this)
                    {
                        newAssets.Add(asset);
                    }
                }
            }
            _references = newAssets.ToArray();
            CacheMap();
        }

        protected void CacheMap()
        {
            var map = new Dictionary<Type, IList<ScriptableObject>>();
            foreach (var item in _references)
            {
                if (item == this)
                {
                    continue;
                }
                Type key = item.GetType();
                if (map.TryGetValue(key, out IList<ScriptableObject> list))
                {
                    list.Add(item);
                }
                else
                {
                    map.Add(key, new List<ScriptableObject> {item});
                }
            }
            Map = new ReadOnlyDictionary<Type, IList<ScriptableObject>>(map);
        }

        public void OnAfterDeserialize()
        {
            CacheMap();
        }
    }
}