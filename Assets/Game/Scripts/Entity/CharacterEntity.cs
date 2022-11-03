using System;
using System.Collections.Generic;
using System.Text;
using Data;
using Data.Character;
using Data.Reactions;
using Data.Trait;
using UnityEngine;

namespace Entity
{
    public class CharacterEntity
    {
        private CharacterName _name;
        private MaskEntity _mask;
        private CharacterName.ImpliedGender _impliedGender;
        private HashSet<TraitData> _traitList;
        private CharacterViewData _viewData;

        public MaskEntity Mask => _mask;
        public CharacterName Name => _name;

        public CharacterName.ImpliedGender ImpliedGender => _impliedGender;
        public HashSet<TraitData> Traits => _traitList;

        public CharacterViewData ViewData => _viewData;

        public virtual string TypeFriendlyName => "Character";

        public event Action<ReactionData> OnReaction;

        public CharacterEntity(MaskEntity mask, CharacterName nameData, HashSet<TraitData> traitList, CharacterViewData viewData)
        {
            _mask = mask;
            _name = nameData;
            _traitList = traitList;
            _impliedGender = nameData.Gender;
            _viewData = viewData;
        }

        public string PrintDebug()
        {
            StringBuilder stringBuilder = new StringBuilder(TypeFriendlyName);
            stringBuilder.Append(" '");
            stringBuilder.Append(_name.Full);
            stringBuilder.Append("' ");
            stringBuilder.Append(CharacterName.AbbreviateGender(_impliedGender));
            stringBuilder.Append(" ");
            stringBuilder.Append(Mask.FriendlyName);
            stringBuilder.Append(" ");
            stringBuilder.Append("[");
            foreach (var traitData in _traitList)
            {
                stringBuilder.Append(traitData.FriendlyName);
                stringBuilder.Append(",");
            }
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        public void DisplayReaction(ReactionData first)
        {
            OnReaction?.Invoke(first);
        }
    }
}