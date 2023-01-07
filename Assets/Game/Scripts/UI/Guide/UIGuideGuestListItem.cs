using Data.Monsters;
using Entity;
using NiftyFramework.UI;
using TMPro;
using UnityEngine;
using UnityUtils;
using FontStyles = TMPro.FontStyles;

namespace UI.Guide
{
    public class UIGuideGuestListItem : MonoBehaviour, IView<CharacterEntity, MonsterEntityTypeDataSet>
    {
        [SerializeField] private TextMeshProUGUI _labelName;
        [SerializeField] private UIGuideGuestListTag _guestListTag;

        private MonsterEntityTypeDataSet _monsterEntityTypeDataSet;

        public void Set(CharacterEntity characterEntity, MonsterEntityTypeDataSet monsterEntityTypeDataSet)
        {
            _monsterEntityTypeDataSet = monsterEntityTypeDataSet;
            gameObject.TrySetActive(true);
            {
                _labelName.SetText(characterEntity.Name.Full);
                _guestListTag.Set(characterEntity.Mask);
                if (characterEntity.TypeGuess.IsEliminated(monsterEntityTypeDataSet.References))
                {
                    _labelName.fontStyle = FontStyles.Strikethrough;
                }
                else
                {
                    _labelName.fontStyle = FontStyles.Normal;
                }
                characterEntity.TypeGuess.OnChange += HandleGuessChange;
            }

        }

        private void HandleGuessChange(CharacterTypeGuess typeGuess)
        {
            if (typeGuess.IsEliminated(_monsterEntityTypeDataSet.References))
            {
                _labelName.fontStyle = FontStyles.Strikethrough;
            }
            else
            {
                _labelName.fontStyle = FontStyles.Normal;
            }
        }

        public void Clear()
        {
            gameObject.TrySetActive(false);
        }
    }
}