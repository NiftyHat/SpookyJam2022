using Data.Trait;
using Entity;
using Generators;
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
        [SerializeField] private ScheduleGenerator _scheduleGenerator;

        public Sprite RevealSprite => _revealSprite;
        public string FriendlyName => _friendlyName;

        public Sprite Icon => _icon;

        public TraitData[] PreferredTraits => _preferredTraits;

        public ScheduleGenerator ScheduleGenerator => _scheduleGenerator;
    }
}