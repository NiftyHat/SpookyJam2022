using System;
using Data.Reactions;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using UI.Cards;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

public class UIButtonReactionView : MonoBehaviour, IView<ReactionData>
{
    [SerializeField] private Button _button;
    [SerializeField][NonNull] private UIReactionView _reactionView;

    public event Action<ReactionData> OnSelected;

    protected ReactionData _data;

    protected void Awake()
    {
        _button.onClick.AddListener(HandleClicked);
    }

    private void HandleClicked()
    {
        OnSelected.Invoke(_data);
    }

    public void Set(ReactionData viewData)
    {
        if (viewData == null)
        {
            Clear();
            return;
        }
        _data = viewData;
        if (gameObject.TrySetActive(true))
        {
            _reactionView.Set(viewData);
        }
    }

    public void Clear()
    {
        gameObject.TrySetActive(false);
    }
}
