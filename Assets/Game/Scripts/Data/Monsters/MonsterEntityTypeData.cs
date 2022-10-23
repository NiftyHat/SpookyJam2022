using Data.Trait;
using Entity;
using UnityEngine;

namespace Data.Monsters
{
    
    public class MonsterEntityTypeData : ScriptableObject, IRevealProvider, IUniqueEntityType
    {
        [SerializeField] private string _friendlyName;
        [SerializeField] private Sprite _revealSprite;
        [SerializeField] private TraitData[] _preferredTraits;

        public Sprite RevealSprite => _revealSprite;
        public string FriendlyName => _friendlyName;

        public TraitData[] PreferredTraits => _preferredTraits;
    }
}