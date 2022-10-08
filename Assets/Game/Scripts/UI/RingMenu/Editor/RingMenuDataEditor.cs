using NiftyScriptableSet;
using UI.RingMenu;
using UnityEditor;

namespace UI.Editor
{
    [CustomEditor(typeof(RingMenuData))]
    public class RingMenuDataEditor : ScriptableSetInspector<RingMenuItemData>
    {
    }
}