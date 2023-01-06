using Context;
using NiftyFramework.Core.Utils;
using UI.Guide;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MonsterGuideButtonWidget : MonoBehaviour
    {
        [SerializeField] [NonNull] private Button _button;
        [SerializeField] [NonNull] private UIGuideBookView _guideView;
        
        private GameStateContext _gameStateContext;

        protected void Start()
        {
            _button.onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            if (_guideView != null)
            {
                _guideView.Show();
            }
        }
    }
}