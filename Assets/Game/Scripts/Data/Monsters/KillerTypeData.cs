using Entity;
using UnityEditor.U2D.Path;
using UnityEngine;

namespace Data.Monsters
{
    [CreateAssetMenu(fileName = "KillerData", menuName = "Game/NPCS/KillerData", order =9)]
    public class KillerTypeData : ScriptableObject, IRevealProvider
    {
        [SerializeField] private string _friendlyName;
        [SerializeField] private Sprite _revealSprite;

        public Sprite RevealSprite => _revealSprite;
        public string FriendlyName => _friendlyName;
    }
}