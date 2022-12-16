using Context;
using NiftyFramework.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MonsterGuideButtonWidget : MonoBehaviour
    {
        [SerializeField] [NonNull] private Button _button;
        [SerializeField] [NonNull] private MonsterGuideMenuWidget _guideWidget;
        
        private GameStateContext _gameStateContext;

        protected void Start()
        {
            _button.onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            if (_guideWidget != null)
            {
                _guideWidget.ShowGuide();
            }
        }
    }
}