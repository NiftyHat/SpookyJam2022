using Data;
using Data.Monsters;

namespace Entity
{
    public class NPCEntity
    {
        public NameData.Entry _name;
        public MaskEntity _mask;
        public NameData.ImpliedGender _impliedGender;
        public MonsterTypeData MonsterTypeData;

        public NPCEntity(MaskEntity mask, MonsterTypeData monsterTypeData, NameData.Entry nameData)
        {
            _mask = mask;
            MonsterTypeData = monsterTypeData;
        }
    }
}