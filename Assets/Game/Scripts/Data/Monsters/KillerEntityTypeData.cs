using Entity;
using UnityEngine;

namespace Data.Monsters
{
    [CreateAssetMenu(fileName = "KillerData", menuName = "Game/Characters/KillerData", order =9)]
    public class KillerEntityTypeData : ScriptableObject, IRevealProvider, IUniqueEntityType
    {
        [SerializeField] private string _friendlyName;
        [SerializeField] private Sprite _revealSprite;

        public Sprite RevealSprite => _revealSprite;
        public string FriendlyName => _friendlyName;
    }
}