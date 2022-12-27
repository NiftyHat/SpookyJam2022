using Data.Monsters;
using UnityEngine;

namespace UI.Filter
{
    public class UIFilterSetMonsterType : UIFilterButtonSetView<MonsterEntityTypeData>
    {
        [SerializeField] private MonsterEntityTypeDataSet _monsterTypes;

        public void Start()
        {
            Set(_monsterTypes.References);
            SetLabel(_defaultTitle);
        }
    }
}