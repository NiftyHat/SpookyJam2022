using Context;
using NiftyFramework.Core.Context;
using UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class TitleScreenView : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private LoadingBarView _loadingBarView;
        [SerializeField] private LayoutGroup _buttonsGroup;

        private GameStateContext _gameStateContext;

        protected void Start()
        {
            _playButton.enabled = false;
            ContextService.Get<GameStateContext>(HandleGameStateContext);

            _loadingBarView.Clear();
        }

        private void HandleGameStateContext(GameStateContext service)
        {
            _playButton.enabled = true;
            //TODO - dsaunders this should be driven by a state system and not whatever the fuck this garbage code is.
            _gameStateContext = service;
            _playButton.onClick.AddListener(HandleClickPlay);
        }

        private void HandleClickPlay()
        {
            _gameStateContext.StartGame(out var loadingOperation);
            if (_loadingBarView != null)
            {
                _buttonsGroup.gameObject.SetActive(false);
                _loadingBarView.Set(loadingOperation);
            }

        }

    }
}
