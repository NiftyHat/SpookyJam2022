using NiftyFramework.UI;
using TMPro;
using UnityEngine;
using UnityUtils;

namespace UI
{
    public class UIAmountLabelView : MonoBehaviour, IView<int, int>
    {
        [SerializeField] private TextMeshProUGUI _labelCurrent;
        [SerializeField] private TextMeshProUGUI _labelTotal;

        public void Set(int current, int total)
        {
            gameObject.TrySetActive(true);
            if (_labelTotal != null)
            {
                _labelCurrent.SetText(current.ToString());
                _labelTotal.SetText(total.ToString());
            }
            else
            {
                _labelCurrent.SetText($"{current}/{total}");
            }
        }

        public void Clear()
        {
            gameObject.TrySetActive(false);
        }
    }
}