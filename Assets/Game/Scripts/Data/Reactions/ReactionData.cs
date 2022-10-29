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
        
        public Sprite Sprite => _sprite;
        public string FriendlyName => _friendlyName;
        public ReactionBubbleView Prefab => _prefab;
    }
}
