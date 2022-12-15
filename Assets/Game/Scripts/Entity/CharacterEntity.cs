using System;
using System.Collections.Generic;
using System.Text;
using Data;
using Data.Character;
using Data.Interactions;
using Data.Location;
using Data.Reactions;
using Data.Trait;
using GameStats;
using Generators;
using Interactions;

namespace Entity
{
    public class CharacterEntity
    {
        private CharacterName _name;
        private MaskEntity _mask;
        private CharacterName.ImpliedGender _impliedGender;
        private HashSet<TraitData> _traitList;
        private CharacterViewData _viewData;

        private LocationData _currentLocation;

        public MaskEntity Mask => _mask;
        public CharacterName Name => _name;

        public CharacterName.ImpliedGender ImpliedGender => _impliedGender;
        public HashSet<TraitData> Traits => _traitList;

        public CharacterViewData ViewData => _viewData;

        public LocationData CurrentLocation => _currentLocation;

        public List<TraitData> TraitGuessList => _traitGuessList;
        private List<TraitData> _traitGuessList;

        private Dictionary<TraitData, Guess> _traitGuessInfo = new Dictionary<TraitData, Guess>();
        
        public Dictionary<TraitData, Guess> TraitGuessInfo => _traitGuessInfo;

        private CharacterTypeGuess _typeGuess = new CharacterTypeGuess();
        public CharacterTypeGuess TypeGuess => _typeGuess;

        private GuestSchedule _schedule;

        public virtual string TypeFriendlyName => "Character";

        public event Action<ReactionData> OnReaction;
        public event Action<LocationData, LocationData> OnLocationChange;
        public CharacterEntity(MaskEntity mask, CharacterName nameData, HashSet<TraitData> traitList, GuestSchedule schedule, CharacterViewData viewData)
        {
            _mask = mask;
            _name = nameData;
            _traitList = traitList;
            _impliedGender = nameData.Gender;
            _viewData = viewData;
            _schedule = schedule;
            _traitGuessList = new List<TraitData>();
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
            stringBuilder.Append(_schedule.ToString());
            return stringBuilder.ToString();
        }

        public void DisplayReaction(ReactionData reactionData)
        {
            OnReaction?.Invoke(reactionData);
        }

        public bool AtLocation(LocationData locationData)
        {
            return _currentLocation == locationData;
        }
        
        public bool AtLocation(LocationData locationData, GameStat phaseStat)
        {
            return _schedule.IsAtLocationDuringPhase(phaseStat.Value, (location => location == locationData));
        }


        public void SetTraitGuess(Dictionary<TraitData, Guess> traitGuessSet)
        {
            _traitGuessInfo = traitGuessSet;
        }

        public void SetPhaseStat(GameStat phaseStat)
        {
            phaseStat.OnChanged += HandlePhaseChanged;
            if (_schedule.TryGetLocation(phaseStat.Value, out var newLocation))
            {
                if (newLocation != _currentLocation)
                {
                    _currentLocation = newLocation;
                    OnLocationChange?.Invoke(newLocation, _currentLocation);
                }
            }
        }

        private void HandlePhaseChanged(int oldValue, int newValue)
        {
            _schedule.TryGetLocation(newValue, out _currentLocation);
        }
    }
}