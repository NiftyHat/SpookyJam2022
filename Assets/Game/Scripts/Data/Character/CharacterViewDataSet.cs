using Entity;
using NiftyFramework.Scripts;
using NiftyScriptableSet;
using UnityEngine;

namespace Data.Character
{
    [CreateAssetMenu(fileName = "CharacterViewDataSet", menuName = "Game/Characters/CharacterViewDataSet", order = 1)]
    public class CharacterViewDataSet : ScriptableSet<CharacterViewData>
    {
        [SerializeField] protected CharacterViewData _dress;
        [SerializeField] protected CharacterViewData _suit;

        public CharacterViewData GetGendered(CharacterName.ImpliedGender gender, System.Random random)
        {
            switch (gender)
            {
                case CharacterName.ImpliedGender.Femme:
                    return _dress;
                case CharacterName.ImpliedGender.Masc:
                    return _suit;
            }
            return GetRandom(random);
        }

        public CharacterViewData GetRandom(System.Random random)
        {
            return References.RandomItem(random);
        }
    }
}