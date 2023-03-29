using NiftyFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class ActiveToggleWidget: MonoBehaviour, IView<bool>
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private bool _isActiveDefault = true;

        public void OnEnable()
        {
            _toggle.onValueChanged.AddListener(HandleToggleChanged);
            Set(_toggle.isOn);
        }

        private void HandleToggleChanged(bool isOn)
        {
            Set(isOn);
        }

        public void Set(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}