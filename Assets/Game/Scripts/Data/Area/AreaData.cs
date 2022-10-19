using UnityEngine;

namespace Data.Area
{
    public class AreaData : ScriptableObject
    {
        [SerializeField] private string _friendlyName;
        [SerializeField] [TextArea] private string _description;
        [SerializeField] private Sprite _icon;
    }
}