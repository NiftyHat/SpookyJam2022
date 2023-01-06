using Data.Monsters;
using Data.Trait;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Guide
{
    public class UIGuideMonsterEntry : MonoBehaviour
    {
        [SerializeField] private MonsterGuidePageSO _pageData;
        [SerializeField] private MonsterEntityTypeData _monsterEntityType;
        
        [SerializeField]
        private TextMeshProUGUI _monsterNameDisplay;
        [SerializeField]
        private Image _monsterPortrait;
        [SerializeField]
        private TextMeshProUGUI _descriptionDisplay;
        [SerializeField]
        private TextMeshProUGUI _hintDisplay;

        [SerializeField] private Image[] _traitDisplay;

        private void Start()
        {
            SetPage(_pageData);
            SetType(_monsterEntityType);
        }
        
        
        [UnityEngine.ContextMenu("EditorSetData")]
        public void EditorSetData()
        {
            SetPage(_pageData);
            SetType(_monsterEntityType);
        }
        
        public void SetPage(MonsterGuidePageSO pageData)
        {
            _monsterNameDisplay.SetText(pageData.monsterName);
            _monsterPortrait.sprite = pageData.image;
            _descriptionDisplay.SetText(pageData.description);
            _hintDisplay.SetText(pageData.hint);
        }

        private void SetType(MonsterEntityTypeData monsterEntityTypeData)
        {
            TraitData[] monsterTraits = monsterEntityTypeData.PreferredTraits;
            for (int i = 0; i < monsterTraits.Length; i++)
            {
                _traitDisplay[i].sprite = monsterTraits[i].Icon;
            }
        }
    }
}