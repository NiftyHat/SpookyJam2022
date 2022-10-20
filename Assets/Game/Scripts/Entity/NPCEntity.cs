using Data;
using Data.Monsters;

namespace Entity
{
    public class NPCEntity
    {
        private NameData.Entry _name;
        private MaskEntity _mask;
        private NameData.ImpliedGender _impliedGender;
        private MonsterTypeData _monsterTypeData;

        public NPCEntity(MaskEntity mask, MonsterTypeData monsterTypeData, NameData.Entry nameData)
        {
            _mask = mask;
            _monsterTypeData = monsterTypeData;
        }
    }
}