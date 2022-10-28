using Interactions;
using NiftyFramework.DataView;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIInteractionButton : MonoBehaviour, IDataView<IInteraction>
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private IconWidget _icon;
        
        public void Clear()
        {
           gameObject.SetActive(false);
        }

        public void Set(IInteraction data)
        {
            if (data == null)
            {
                return;
            }
            gameObject.SetActive(true);
            _label.SetText(data.MenuItem.FriendlyName);
            _icon.SetSprite(data.MenuItem.Icon);
        }
    }
}