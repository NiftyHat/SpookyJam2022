using System.Collections;
using UI.Widgets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Screens
{
    public class TitleScreenView : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private LoadingBarView _loadingBarView;
        [SerializeField] private LayoutGroup _buttonsGroup;

        protected void Start()
        {
            _playButton.onClick.AddListener(HandleClickPlay);
            _loadingBarView.Clear();
        }

        private void HandleClickPlay()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
            if (_loadingBarView != null)
            {
                _buttonsGroup.gameObject.SetActive(false);
                _loadingBarView.Set(asyncLoad);
            }
        }

    }
}
