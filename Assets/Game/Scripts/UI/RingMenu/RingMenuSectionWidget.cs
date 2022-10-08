using UnityEngine;
using UnityEngine.UI;

namespace UI.RingMenu
{
    public class RingMenuSectionWidget : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _icon;

        private int _index = 0;

        public Image Icon => _icon;
        public Image Background => _background;

        public int Index => _index;

        public void Set(RingMenu.RingMenuItemData menuItem, int index)
        {
            _icon.sprite = menuItem.Icon;
            _index = index;
        }

        public float GetIconDistance()
        {
            return Vector3.Distance(this.transform.position, _icon.transform.position);
        }

        public void PointerExit()
        {
            _background.color = Color.black;
        }

        public void PointerOver()
        {
            _background.color = Color.gray;
        }
    }
}
