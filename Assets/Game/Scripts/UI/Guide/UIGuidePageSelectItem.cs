using BookCurlPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Guide
{
    public class UIGuidePageSelectItem: MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private int _page;
        [SerializeField] private AutoFlip _bookFlip;

        protected void Start()
        {
            if (_button != null)
            {
                _button.onClick.AddListener(OnClick);
            }
        }

        private void OnClick()
        {
            int paper = _page / 2;
            if (_bookFlip != null)
            {
                _bookFlip.enabled = true;
                _bookFlip.StartFlipping(paper);
            }
        }
    }
}