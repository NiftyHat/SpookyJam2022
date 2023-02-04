using NiftyFramework.Core.Data;
using TMPro;
using UnityEngine;

namespace NiftyFramework
{
    public static class TMPExtensionMethods
    {
        public static void SetColor(this TextMeshProUGUI str, Optional<Color> color)
        {
            if (color.Enabled)
            {
                str.color = color.Value;
            }
        }
    }
}