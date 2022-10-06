using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RingMenuSectionWidget : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _icon;

        public Image Icon => _icon;
        public Image Background => _background;

        public void Set(RingMenuItemData menuItem)
        {
            _icon.sprite = menuItem.Icon;
        }

        public float GetIconDistance()
        {
            return Vector3.Distance(this.transform.position, _icon.transform.position);
        }
    }
}
