using System;
using System.Collections.Generic;
using NiftyFramework.UI;
using NiftyFramework.UnityUtils;
using UnityEngine;
using UnityUtils;

namespace UI.RingMenu
{
    public class RingMenuWidget : MonoBehaviour, IView<IRingMenuViewData>
    {
        protected IRingMenuViewData _viewData;

        [SerializeField] protected RingMenuSectionWidget _sectionPrefab;
        [SerializeField] protected float _sectionSpacing = 1f;

        private MonoPool<RingMenuSectionWidget> _sectionPool;
        private readonly List<RingMenuSectionWidget> _sectionWidgets = new List<RingMenuSectionWidget>();
        private int _lastSelectedSection = -1;
        private RingMenuSectionWidget _lastSelectedWidget;

        private float _stepLength = 0;
        private float _maxSelectionDistance = 100f;
        public event Action<RingMenuSectionWidget> OnSelected;
        public event Action<RingMenuSectionWidget> OnHover;

        // Start is called before the first frame update
        void Start()
        {
            _sectionPool = new MonoPool<RingMenuSectionWidget>(_sectionPrefab);
        }
        
        // Update is called once per frame
        void Update()
        {
            if (_lastSelectedWidget != null && Input.GetMouseButtonUp(0))
            {
                OnSelected?.Invoke(_lastSelectedWidget);
            }
                    
            var mouseDistance = Vector3.Distance(transform.position, Input.mousePosition);
            if (mouseDistance > _maxSelectionDistance && _lastSelectedWidget != null)
            {
                _lastSelectedWidget.PointerExit();
                _lastSelectedWidget = null;
                return;
            }
            var mouseAngle =
                NormalizeAngle(
                    Vector3.SignedAngle(Vector3.up, Input.mousePosition - transform.position, Vector3.forward) +
                    _stepLength / 2f);
            int mouseOverSectionIndex = (int)(mouseAngle / _stepLength);
            if (mouseOverSectionIndex != _lastSelectedSection)
            {
                _lastSelectedSection = mouseOverSectionIndex;
                if (mouseOverSectionIndex >= 0 && mouseOverSectionIndex <= _sectionWidgets.Count)
                {
                    var selectedItem = _sectionWidgets[mouseOverSectionIndex];
                    if (_lastSelectedWidget != selectedItem)
                    {
                        if (_lastSelectedWidget != null)
                        {
                            _lastSelectedWidget.PointerExit();
                            _lastSelectedWidget = null; 
                        }
                        selectedItem.PointerOver();
                        OnHover?.Invoke(selectedItem);
                        _lastSelectedWidget = selectedItem;
                    }
                }
            }
        }

        private float NormalizeAngle(float a) => (a + 360f) % 360;
        public void Open(Vector2 screenPosition)
        {
            gameObject.transform.position = screenPosition;
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Set(IRingMenuViewData viewData)
        {
            _viewData = viewData;
            int itemCount = _viewData.Count;
            if (itemCount == 0)
            {
                return;
            }

            _stepLength = 360f / itemCount;
            float iconDist = _sectionPrefab.GetIconDistance();
            float length = _viewData.Items.Count;

            for (int i = 0; i < length; i++)
            {
                var sectionData = _viewData.Items[i];
                _sectionPool.TryGet(out var sectionInstance);
                //var sectionInstance = Instantiate(_sectionPrefab, Vector3.one, Quaternion.identity, transform);
                sectionInstance.Background.fillAmount = (1f / length) - _sectionSpacing / 360f;
                sectionInstance.Background.transform.localPosition = Vector3.zero;
                sectionInstance.Background.transform.localRotation = Quaternion.Euler(0, 0, -_stepLength / 2f + _sectionSpacing + i * _stepLength);
                sectionInstance.Background.color = Color.black;

                sectionInstance.Icon.transform.localPosition = Vector3.zero +
                                                               Quaternion.AngleAxis(i * _stepLength, Vector3.forward) *
                                                               Vector3.up * iconDist;
                sectionInstance.Set(sectionData, i);
                _sectionWidgets.Add(sectionInstance);
            }
        }

        public void Clear()
        {
            _sectionWidgets.Clear();
        }
    }
}
