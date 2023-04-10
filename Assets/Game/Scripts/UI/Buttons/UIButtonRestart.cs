using Context;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using UnityEngine;
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
            ContextService.Get<GameStateContext>(service =>
            {
                service.RestartGame();
            });
        }
    }
}