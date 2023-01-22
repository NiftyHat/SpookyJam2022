using System.Collections.Generic;
using System.Linq;
using Data.Monsters;
using NiftyFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace UI
{
    public class UIGuessInfoView : MonoBehaviour, IView<CharacterTypeGuess>
    {
        [SerializeField] private MonsterEntityTypeDataSet _monsterEntityTypeData;
        [SerializeField] private UIAmountLabelView _amountView;
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _noGuessSprite;
        [SerializeField] private Sprite _eliminatedSprite;
        
        public void Set(CharacterTypeGuess guessInfo)
        {
            if (guessInfo != null)
            {
                guessInfo.OnChange += HandleGuessChanged;
                HandleGuessChanged(guessInfo);
            }
            else
            {
                _icon.TrySetActive(false);
                _amountView.Set(_monsterEntityTypeData.References.Count, _monsterEntityTypeData.References.Count);
            }
        }

        private void HandleGuessChanged(CharacterTypeGuess guessInfo)
        {
            var monsterDataList = _monsterEntityTypeData.References;
            bool useYesItems = false;
            HashSet<MonsterEntityTypeData> possibleMonsters = new(monsterDataList);
            foreach (var item in monsterDataList)
            {
                Guess guessValue = guessInfo.GetMonsterGuess(item);
                switch (guessValue)
                {
                    case Guess.No:
                        possibleMonsters.Remove(item);
                        break;
                    case Guess.Yes:
                        useYesItems = true;
                        //if (useYesItems == false)
                        //{
                            //possibleMonsters.Clear();
                        //}
                        possibleMonsters.Add(item);
                        break;
                }
            }

            if (possibleMonsters.Count == 1 && useYesItems)
            {
                _icon.color = Color.white;
            }
            else if (possibleMonsters.Count == 0)
            {
                _icon.color = Color.red;
            }
            else
            {
                _icon.color = Color.black;
            }

            if (possibleMonsters.Count <= 1)
            {
                _amountView.Clear();
                if (possibleMonsters.Count == 1)
                {
                    var monsterToDisplay = possibleMonsters.FirstOrDefault();
                    if (_icon.TrySetActive(monsterToDisplay != null))
                    {
                        _icon.sprite = monsterToDisplay.Icon;
                    }
                }
                else
                {
                    _icon.sprite = _eliminatedSprite;
                }
            }
            else
            {
               
                _icon.sprite = _noGuessSprite;
                _amountView.Set(possibleMonsters.Count, monsterDataList.Count);
            }
            _icon.TrySetActive(_icon.sprite != null);
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}