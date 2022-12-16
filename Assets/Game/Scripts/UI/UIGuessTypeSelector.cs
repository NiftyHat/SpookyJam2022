using System.Collections.Generic;
using System.Linq;
using Data.Monsters;
using NiftyFramework.DataView;
using UnityEngine;

namespace UI
{
    public class UIGuessTypeSelector : MonoBehaviour, IDataView<CharacterTypeGuess>
    {
        [SerializeField] protected UILabeledGuessSelector _labeledGuessSelectorKiller;
        [SerializeField] protected UILabeledGuessSelector _labeledGuessSelectorHuman;
        [SerializeField] protected UIMonsterGuessList _monsterGuessList;
        private CharacterTypeGuess _data;
        
        void Start()
        {
            _monsterGuessList.OnChangeSelection += HandleMonsterGuessChanged;
        }

        private void HandleDataChange(CharacterTypeGuess guessData)
        {
            if (guessData == null)
            {
                return;
            }
        }
        
        private void HandleMonsterGuessChanged(Dictionary<MonsterEntityTypeData, Guess> guessData)
        {
            _data.SetMonster(guessData);
        }

        public void Clear()
        {
            _monsterGuessList.Clear();
            _labeledGuessSelectorHuman.SetGuess(Guess.No);
            _labeledGuessSelectorHuman.SetGuess(Guess.No);
        }

        public void Set(CharacterTypeGuess data)
        {
            if (data == null)
            {
                Clear();
                _data = null;
                return;
            }
            _data = data;
            _data.OnChange += HandleDataChange;
            if (_data.GuessMonster != null)
            {
                _monsterGuessList.Set(data.GuessMonster);
            }
            else
            {
                _monsterGuessList.Clear();
            }
        }
    }
}
