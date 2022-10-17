using Data;
using NiftyFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Screens
{
    public class GameOverScreenView : MonoBehaviour, IView<GameOverReasonData>
    {
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _titleButton;
        [SerializeField] private TextMeshProUGUI _labelGameOverReason;

        protected void Start()
        {
            _retryButton.onClick.AddListener(HandleRetryClick);
            _quitButton.onClick.AddListener(HandleQuitClick);
            _titleButton.onClick.AddListener(HandleTitleClick);
#if UNITY_IPHONE
            _quitButton.gameObject.SetActive(false);
#endif
        }

        private void HandleTitleClick()
        {
            SceneManager.LoadScene(0);
        }

        private void HandleQuitClick()
        {
            #if UNITY_IPHONE
            SceneManager.LoadScene(0);
            return;
            #endif
            Application.Quit();
        }

        private void HandleRetryClick()
        {
            SceneManager.LoadScene(1);
        }

        public void Set(GameOverReasonData gameOver)
        {
            if (gameOver != null)
            {
                _labelGameOverReason.gameObject.SetActive(true);
                _labelGameOverReason.SetText(gameOver.Description);
            }
            else
            {
                _labelGameOverReason.gameObject.SetActive(false);
            }
        }

        public void Clear()
        {
            if (_labelGameOverReason != null)
            {
                _labelGameOverReason.gameObject.SetActive(false);
            }
            
        }
    }
}
