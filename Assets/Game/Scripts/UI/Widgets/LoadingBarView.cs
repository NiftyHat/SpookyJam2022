using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class LoadingBarView : MonoBehaviour, IView<AsyncOperation>
    {
        private AsyncOperation _loadingOperation;
        [SerializeField][NonNull] private Slider _sliderProgress;
        [SerializeField] private TextMeshProUGUI _labelProgress;
        
        public void Set(AsyncOperation loadingOperation)
        {
            if (loadingOperation != null && !loadingOperation.isDone)
            {
                _loadingOperation = loadingOperation;
                gameObject.SetActive(true);
            }
            
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }

        protected void Update()
        {
            if (_loadingOperation == null)
            {
                return;
            }
            if (!_loadingOperation.isDone)
            {
                _sliderProgress.value = _loadingOperation.progress;
                if (_labelProgress != null)
                {
                    float percentage = _loadingOperation.progress * 100f;
                    _labelProgress.SetText(percentage.ToString("{0:P0}"));
                }
            }
            else
            {
                if (_labelProgress != null)
                {
                    _labelProgress.SetText("100%");
                }
                _sliderProgress.value = 1.0f;
            }
        }
    }
}
