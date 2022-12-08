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
            _labeledGuessSelectorKiller.OnValueChanged += HandleKillerGuess;
            _labeledGuessSelectorHuman.OnValueChanged += HandleHumanGuess;
        }

        private void HandleDataChange(CharacterTypeGuess guessData)
        {
            if (guessData == null)
            {
                return;
            }
        }

        private void HandleHumanGuess(Guess guessValue)
        {
            _data.SetHuman(guessValue);
        }

        private void HandleKillerGuess(Guess guessValue)
        {
            _data.SetKiller(guessValue);
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
            _labeledGuessSelectorHuman.Set("Human", _data.Human);
            _labeledGuessSelectorKiller.Set("Killer", _data.Killer);
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
