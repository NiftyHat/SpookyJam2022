using Data.Location;
using NiftyFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace UI.Guide
{
    public class UIGuideLocationEntry : MonoBehaviour, IView<LocationData>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private LocationData _locationData;

        public void Start()
        {
            Set(_locationData);
        }

        [UnityEngine.ContextMenu("EditorSetData")]
        public void EditorSetData()
        {
            Set(_locationData);
        }

        public void Set(LocationData viewData)
        {
            gameObject.TrySetActive(true);
            _title.SetText(viewData.FriendlyName);
            _icon.sprite = viewData.Icon;
        }

        public void Clear()
        {
            gameObject.TrySetActive(false);
        }
    }
}