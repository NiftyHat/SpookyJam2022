using BookCurlPro;
using Context;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Guide
{
    public class UIGuideBookView : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField][NonNull] private GameObject _panel;
        [SerializeField][NonNull] private AutoFlip _bookFlip;

        protected void Awake()
        {
            ContextService.Get<GameStateContext>(gameContext =>
            {
                gameContext.OnGuideBookOpenChanged += HandleShowGuideBook;
            });
            _panel.SetActive(false);
        }

        private void HandleShowGuideBook(bool isOpen, int pageToDisplay = -1)
        {
            if (isOpen)
            {
                if (pageToDisplay >= 0)
                {
                    Show(pageToDisplay);
                }
                else
                {
                    Show();
                }
            }
            else
            {
                Hide();
            }
        }

        protected void Start()
        {
            _closeButton.onClick.AddListener(OnClickClose);
        }

        private void OnClickClose()
        {
            _panel.SetActive(false);
        }
        
        public void Show()
        {
            _panel.SetActive(true);
        }

        public void Hide()
        {
            _panel.SetActive(false);
        }
        
        public void Show(int page)
        {
            _panel.SetActive(true);
            if (page >= 0)
            {
                int paper = page / 2;
                if (_bookFlip != null)
                {
                    _bookFlip.enabled = true;
                    _bookFlip.StartFlipping(paper);
                }
            }
        }
    }
}