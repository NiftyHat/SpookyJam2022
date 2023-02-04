using NiftyFramework.Core.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class UIButtonRestart : MonoBehaviour
    {
        [SerializeField][NonNull] private Button _button;
        
        protected void Start()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            SceneManager.LoadScene(0);
        }
    }
}