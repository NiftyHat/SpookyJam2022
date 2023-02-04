using NiftyFramework.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class UIButtonQuit : MonoBehaviour
    {
        [SerializeField][NonNull] private Button _button;
        
        protected void Start()
        {
            
            #if UNITY_IPHONE
                gameObject.SetActive(false);
            return;
            #endif
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            Application.Quit();
        }
    }
}