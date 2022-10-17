using NiftyFramework.Core.Services;
using NiftyFramework.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NiftyFramework
{
    public class AddressableIndex<TAssetIndex> : AssetService<TAssetIndex> where TAssetIndex : ScriptableObject, IAssetIndex
    {
        public class Config
        {
            public readonly string AddressableKey;

            public Config(string addressableKey)
            {
                AddressableKey = addressableKey;
            }
        }
        
        private OnReady _onReady;
        private readonly Config _config;

        public AddressableIndex(Config config)
        {
            _config = config;
        }
        
        public AddressableIndex(string indexPath)
        {
            _config = new Config(indexPath);
        }
        
        public override void Init(NiftyService.OnReady onReady)
        {
            _onReady = onReady;
            AsyncOperationHandle<TAssetIndex> assetIndexHandle =
                Addressables.LoadAssetAsync<TAssetIndex>(_config.AddressableKey);//Addressables.LoadAsset<GameAssetIndex>(_config.AddressableKey);
            assetIndexHandle.Completed += OnAssetIndexLoadComplete;
        }

        private void OnAssetIndexLoadComplete(AsyncOperationHandle<TAssetIndex> handle) 
        {
            if (handle.Status == AsyncOperationStatus.Succeeded) 
            {
                _assetIndex = handle.Result;
                _onReady.Invoke();
            }
        }
        
        public override TAsset Get<TAsset>()
        {
            if (_assetIndex == null)
            {
                Debug.LogError($"{nameof(AddressableIndex<TAssetIndex>)} {nameof(_assetIndex)} is null, ensure {nameof(Init)} called first");
                return default;
            }
            
            if (_assetIndex.TryGet<TAsset>(out var asset))
            {
                return asset;
            }
            else
            {
                Debug.LogError($"{nameof(AddressableIndex<TAssetIndex>)} failed to load asset type {typeof(TAsset).Name}");
            }
            return default;
        }
        
    }
}