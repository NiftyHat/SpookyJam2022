using NiftyFramework.Core;
using UnityEngine;

namespace Data.Reactions
{
    public class ReactionData : ScriptableObject
    {
        [SerializeField][SpritePreview] protected Sprite _sprite;
        [SerializeField] protected string _friendlyName;
        [SerializeField] protected string _animationName;
        [SerializeField] protected ReactionBubbleView _prefab;
        [SerializeField] protected bool _isMiss;
        [SerializeField] protected Sprite _background;
        
        public Sprite Sprite => _sprite;
        public Sprite Background => _background;
        public string FriendlyName => _friendlyName;
        public ReactionBubbleView Prefab => _prefab;

        public bool isMiss => _isMiss;
    }
}
