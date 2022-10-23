using NiftyScriptableSet;
using UnityEngine;

namespace Data.Mask
{
    [CreateAssetMenu(fileName = "MaskDataSet", menuName = "Game/Mask/MaskDataSet", order = 3)]
    public class MaskDataSet : ScriptableSet<MaskData>
    {
        [SerializeField] private Color[] _colours;
    }
}