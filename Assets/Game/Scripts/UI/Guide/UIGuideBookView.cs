using UnityEngine;
using UnityEngine.UI;

namespace UI.Guide
{
    public class UIGuideBookView : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        protected void Start()
        {
            _closeButton.onClick.AddListener(OnClickClose);
        }

        private void OnClickClose()
        {
            gameObject.SetActive(false);
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Show(int page)
        {
            gameObject.SetActive(true);
        }
    }
}