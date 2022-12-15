using Data.Trait;
using Entity;
using NiftyFramework.Core;
using UnityEngine;

namespace Data.Monsters
{
    
    public class MonsterEntityTypeData : ScriptableObject, IRevealProvider, IUniqueEntityType
    {
        [SerializeField] private string _friendlyName;
        [SerializeField] private Sprite _revealSprite;
        [SerializeField] private TraitData[] _preferredTraits;
        [SerializeField][SpritePreview] private Sprite _icon;

        public Sprite RevealSprite => _revealSprite;
        public string FriendlyName => _friendlyName;

        public Sprite Icon => _icon;

        public TraitData[] PreferredTraits => _preferredTraits;
    }
}