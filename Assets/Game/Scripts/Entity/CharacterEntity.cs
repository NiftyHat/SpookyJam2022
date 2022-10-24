using System.Collections.Generic;
using Data.Trait;

namespace Entity
{
    public class CharacterEntity
    {
        private CharacterName _name;
        private MaskEntity _mask;
        private CharacterName.ImpliedGender _impliedGender;
        private List<TraitData> _traitList;

        public MaskEntity Mask => _mask;
        public CharacterName Name => _name;

        public CharacterName.ImpliedGender ImpliedGender => _impliedGender;
        
        public CharacterEntity(MaskEntity mask, CharacterName nameData, List<TraitData> traitList)
        {
            _mask = mask;
            _name = nameData;
            _traitList = traitList;
        }
    }
}