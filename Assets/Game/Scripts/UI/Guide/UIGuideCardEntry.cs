using System.Linq;
using Data;
using Data.Trait;
using UI.Cards;
using UnityEngine;

namespace UI.Guide
{
    public class UIGuideCardEntry : MonoBehaviour
    {
        [SerializeField] private UICardTraitView _card;
        [SerializeField] private TraitData _traitData;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Guess _guess;

        public void Start()
        {
            if (_traitData != null && _playerData != null)
            {
                if (_playerData.TryGetReactionAbilities(_traitData, out var reactionList, out var abilityList))
                {
                    _card.Set(_traitData, reactionList.ToList(), abilityList.ToList());
                    _card.SetGuess(_guess);
                }
            }
        }
    }
}