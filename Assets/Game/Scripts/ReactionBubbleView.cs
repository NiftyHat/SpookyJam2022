using Context;
using Data.Reactions;
using NiftyFramework.Core.Context;
using NiftyFramework.DataView;
using UnityEngine;

public class ReactionBubbleView : MonoBehaviour, IDataView<ReactionData>
{
    [SerializeField] private SpriteRenderer _emoteSprite;
    [SerializeField] private Animator _animator;
    
    private static readonly int ShowingEmote = Animator.StringToHash("ShowingEmote");
    private GameStateContext _gameStateContext;

    public void Start()
    {
        ContextService.Get<GameStateContext>(HandleGameStateContext);
    }

    private void HandleGameStateContext(GameStateContext service)
    {
        _gameStateContext = service;
        _gameStateContext.OnClearReactions += HandleClearReactions;
    }

    private void HandleClearReactions()
    {
        _animator.SetBool(ShowingEmote, false);
    }

    public void Clear()
    {
        _animator.SetBool(ShowingEmote, false);
    }

    public void Set(ReactionData data)
    {
        _emoteSprite.sprite = data.Sprite;
        _animator.SetBool(ShowingEmote, true);
    }
}