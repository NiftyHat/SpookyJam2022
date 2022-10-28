using NiftyFramework.Core;
using UnityEngine;

namespace Data.Reactions
{
    public class ReactionData : ScriptableObject
    {
        [SerializeField][SpritePreview] protected Texture2D _texture;
        [SerializeField] protected string _friendlyName;
        [SerializeField] protected string _animationName;

        public Texture2D Texture => _texture;
        public string FriendlyName => _friendlyName;
    }
}
