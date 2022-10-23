using System.Collections.Generic;
using System.Text;
using Data.Mask;
using Data.Style;
using Entity;
using NiftyFramework.Scripts;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "MaskGenerator", menuName = "Game/Mask/MaskGenerator", order = 1)]
    public class MaskGenerator : ScriptableObject
    {
        public class MaskPool
        {
            private Dictionary<ColorStyleData, List<MaskEntity>> _colorLookup;
            private Dictionary<MaskData, List<MaskEntity>> _maskLookup;
            private List<MaskEntity> _all;

            public MaskPool(List<MaskData> maskDataList, List<ColorStyleData> colorStyleList)
            {
                _colorLookup = new Dictionary<ColorStyleData,  List<MaskEntity>>();
                _maskLookup = new Dictionary<MaskData,  List<MaskEntity>>();
                _all = new List<MaskEntity>();
                foreach (var maskType in maskDataList)
                {
                    foreach (var colorStyle in colorStyleList)
                    {
                        MaskEntity entity = new MaskEntity(maskType, colorStyle);
                        if (_maskLookup.TryGetValue(maskType, out var cacheMaskList))
                        {
                            cacheMaskList.Add(entity);
                        }
                        else
                        {
                            _maskLookup[maskType] = new List<MaskEntity>() { entity };
                        }
                        if (_colorLookup.TryGetValue(colorStyle, out var cacheColorsList))
                        {
                            cacheColorsList.Add(entity);
                        }
                        else
                        {
                            _colorLookup[colorStyle] = new List<MaskEntity>() { entity };
                        }
                        _all.Add(entity);
                    }
                }
            }

            public bool TryGet(out MaskEntity entity, System.Random random = null)
            {
                if (_all != null)
                {
                    entity = _all.RandomItem(random);
                    if (entity != null)
                    {
                        Remove(entity);
                        return true;
                    }
                }
                entity = null;
                return false;
            }

            private void Remove(MaskEntity entity)
            {
                if (_colorLookup.TryGetValue(entity.ColorStyleData, out var colorCacheList))
                {
                    colorCacheList.Remove(entity);
                }
                if (_maskLookup.TryGetValue(entity.MaskData, out var maskTypeCacheList))
                {
                    maskTypeCacheList.Remove(entity);
                }
                _all.Remove(entity);
            }

            public IReadOnlyList<MaskEntity> All()
            {
                return _all;
            }

            public int Count => _all != null ? _all.Count : 0;

            public IReadOnlyList<MaskEntity> All(MaskData maskType)
            {
                if (_maskLookup.TryGetValue(maskType, out var maskTypeCacheList))
                {
                    return maskTypeCacheList;
                }
                return null;
            }
            
            public IReadOnlyList<MaskEntity> All(ColorStyleData color)
            {
                if (_colorLookup.TryGetValue(color, out var colorCacheList))
                {
                    return colorCacheList;
                }
                return null;
            }

            public string PrintDebug()
            {
                var sb = new StringBuilder("Mask Pool");
                sb.AppendLine();
                foreach (var item in _all)
                {
                    sb.AppendLine(item.FriendlyName);
                }
                return sb.ToString();
            }
        }
        
        [SerializeField] protected MaskDataSet _maskData;
        [SerializeField] protected ColorStyleDataSet _maskColorData;

        public MaskPool GetPool()
        {
            return new MaskPool(_maskData.References, _maskColorData.References);
        }
        
        [ContextMenu("Test")]
        public void Test()
        {
            System.Random random = new System.Random();
            var testPool = GetPool();
            Debug.Log(testPool.PrintDebug());
        }
    }
}