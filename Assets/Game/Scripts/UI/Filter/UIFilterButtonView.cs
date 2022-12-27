using System;
using Data.Menu;
using NiftyFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace UI.Filter
{
    public class UIFilterButtonView : MonoBehaviour, IView<UIFilterButtonView.Data>
    {
        public abstract class Data
        {
            protected Sprite _icon;
            protected string _friendlyName;
            public Sprite Icon => _icon;
            public string FriendlyName => _friendlyName;
        }

        public class Data<TFilterItem> : Data
        {
            private readonly TFilterItem _item;
            public TFilterItem Item => _item;

            public Data(TFilterItem item, IMenuItem menuItem)
            {
                _item = item;
                _icon = menuItem.Icon;
                _friendlyName = menuItem.FriendlyName;
            }
            
            public Data(TFilterItem item)
            {
                _item = item;
                if (item is IMenuItemProvider provider)
                {
                    _icon = provider.MenuItem.Icon;
                    _friendlyName = provider.MenuItem.FriendlyName;
                }
            }
        }

        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;

        public event Action<Data> OnSelected;

        protected Data _data;

        protected void Awake()
        {
            _button.onClick.AddListener(HandleClicked);
        }

        private void HandleClicked()
        {
            OnSelected.Invoke(_data);
        }

        public void Set(Data viewData)
        {
            if (viewData == null)
            {
                Clear();
                return;
            }
            _data = viewData;
            if (gameObject.TrySetActive(true))
            {
                _icon.sprite = _data.Icon;
            }
        }

        public void Clear()
        {
            gameObject.TrySetActive(false);
        }
    }
}
