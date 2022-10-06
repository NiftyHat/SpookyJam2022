using System;
using UnityEngine;
using UnityUtils;

namespace UI.RingMenu
{
    public class RingMenuWidget : MonoBehaviour
    {
        [SerializeField] protected RingMenuData _ringMenu;
        protected RingMenuModel _model;

        [SerializeField] RingMenuSectionWidget _sectionPrefab;
        [SerializeField] protected float _sectionSpacing = 1f;


        public event Action OnSelected;

        // Start is called before the first frame update
        void Start()
        {
            _model = _ringMenu.Create();
            int itemCount = _model.Count;
            if (itemCount == 0)
            {
                return;
            }

            float stepLength = 360f / itemCount;
            float iconDist = _sectionPrefab.GetIconDistance();
            float length = _ringMenu.References.Count;

            for (int i = 0; i < length; i++)
            {
                var sectionData = _ringMenu.References[i];
                var sectionInstance = Instantiate(_sectionPrefab, Vector3.one, Quaternion.identity, transform);
                sectionInstance.Background.fillAmount = (1f / length) - _sectionSpacing / 360f;
                sectionInstance.Background.transform.localPosition = Vector3.zero;
                sectionInstance.Background.transform.localRotation = Quaternion.Euler(0, 0, -stepLength / 2f + _sectionSpacing + i * stepLength);
                sectionInstance.Background.color = Color.black;

                sectionInstance.Icon.transform.localPosition = Vector3.zero +
                                                               Quaternion.AngleAxis(i * stepLength, Vector3.forward) *
                                                               Vector3.up * iconDist;
                sectionInstance.Set(sectionData);
            }
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
