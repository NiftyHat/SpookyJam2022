using Context;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIMonsterGuideButtonWidget : MonoBehaviour, IDataView<int>
    {
        [SerializeField] [NonNull] private Button _button;
        [SerializeField] private int _page = -1;
        private GameStateContext _gameStateContext;

        protected void Start()
        {
            _button.onClick.AddListener(HandleClick);
        }
        
        protected void Awake()
        {
            ContextService.Get<GameStateContext>(gameContext =>
            {
                _gameStateContext = gameContext;
            });
        }


        private void HandleClick()
        {
           _gameStateContext.SetGuideBookOpen(true, _page);
        }

        public void Clear()
        {
            //do nothing
        }

        public void Set(int page)
        {
            _page = page;
        }
    }
}