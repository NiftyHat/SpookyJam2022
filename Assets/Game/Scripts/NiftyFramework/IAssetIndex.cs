using System.Collections;

namespace NiftyFramework
{
    public interface IAssetIndex
    {
        public bool TryGet<TAsset>(out TAsset asset);
        //public bool TryGet<TAsset>(out IList<TAsset> assets);
    }
}