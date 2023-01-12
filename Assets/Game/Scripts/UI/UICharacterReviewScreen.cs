using System.Collections.Generic;
using Context;
using Data;
using Data.Interactions;
using Data.Monsters;
using Data.Reactions;
using Data.Trait;
using Entity;
using NiftyFramework.Core.Context;
using NiftyFramework.UI;
using UI;
using UI.Cards;
using UI.Filter;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterReviewScreen : MonoBehaviour, IView<CharacterEntity>
{
    [SerializeField] private UICardCharacterView _cardCharacterView;
    [SerializeField] private TraitDataSet _traitData;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private LayoutGroup _cardsActive;
    [SerializeField] private LayoutGroup _cardsPassive;

    [SerializeField] private Button _buttonNext;
    [SerializeField] private Button _buttonPrevious;
    [SerializeField] private UICardSpreadView _cardSpreadView;
    [SerializeField] private Button _clearFilterButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private UIReactionView _lastReactionView;
    [SerializeField] private UIGuessTypeSelector _guessTypeSelector;

    private List<UIFilterButtonSetView> _filterViews;
    [SerializeField] private UIFilterSetAbilityTrigger _filterTriggerAbility;
    [SerializeField] private UIFilterSetMonsterType _filterMonsterType;
    [SerializeField] private UIFilterGuessState _filterGuessState;

    [SerializeField] private GameObject _root;

    private GameStateContext _gameStateContext;
    private CharacterEntity _currentCharacter;
    private IReadOnlyList<CharacterEntity> _characterEntities;
    private GameStateContext.LastInteractionData _lastInteractionData;

    private void Start()
    {
        ContextService.Get<GameStateContext>((context) =>
        {
            _gameStateContext = context;
            if (_currentCharacter == null)
            {
                _characterEntities = _gameStateContext.CharacterEntities;
                Set(_characterEntities[0]);
            }
            _gameStateContext.OnTriggerCharacterReview += HandleTriggerCharacterReview;
        });

        List<AbilityReactionTriggerData> reactionAbilityList = _playerData.GetInteractionDataList<AbilityReactionTriggerData>();
        _filterTriggerAbility.Set(reactionAbilityList);

        _filterTriggerAbility.OnClick += HandleSelectedFilter;
        _filterMonsterType.OnClick += HandleSelectedFilter;
        _filterGuessState.OnClick += HandleSelectedFilter;

        _filterViews = new List<UIFilterButtonSetView>();
        
        _filterViews.Add(_filterTriggerAbility);
        _filterViews.Add(_filterMonsterType);
        _filterViews.Add(_filterGuessState);
        
        _cardSpreadView.Set(_traitData.References, _playerData);
        if (_currentCharacter != null)
        {
            _cardSpreadView.SetGuessInfo(_currentCharacter.TraitGuessInfo);
            _guessTypeSelector.Set(_currentCharacter.TypeGuess);
        }
        
        _cardSpreadView.OnTraitGuessesChanged += HandleTraitGuessesChanged;

        _buttonNext.onClick.AddListener(HandleClickNext);
        _buttonPrevious.onClick.AddListener(HandleClickPrevious);
        _closeButton.onClick.AddListener(HandleClose);
    }


    private void HandleClose()
    {
        _root.SetActive(false);
    }

    private void HandleTriggerCharacterReview(CharacterEntity characterEntity)
    {
        _root.SetActive(true);
        characterEntity.WasSeen.Value = true;
        Set(characterEntity);
    }

    private void HandleClickClear()
    {
        _cardSpreadView.ClearSelection();
    }

    private void HandleTraitGuessesChanged(Dictionary<TraitData, Guess> guesses)
    {
        _currentCharacter.SetTraitGuess(guesses);
    }


    private void HandleSelectedFilter(UIFilterButtonSetView view, UIFilterButtonView.Data buttonData)
    {
        foreach (var item  in _filterViews)
        {
            if (item != null && item != view)
            {
                item.Clear();
            }
        }
        if (buttonData is UIFilterButtonView.Data<ReactionData> reactionButton)
        {
            DoFilter(reactionButton.Item);
        }
        if (buttonData is UIFilterButtonView.Data<AbilityReactionTriggerData> abilityButton)
        {
            DoFilter(abilityButton.Item);
        }
        if (buttonData is UIFilterButtonView.Data<MonsterEntityTypeData> monsterButton)
        {
            DoFilter(monsterButton.Item);
        }
        if (buttonData is UIFilterButtonView.Data<UIFilterGuessState.ItemData> selectedButton)
        {
            DoFilter(selectedButton.Item);
        }
    }

    private void DoFilter(UIFilterGuessState.ItemData selectedData)
    {
        _cardSpreadView.WithAll(item =>
        {
            if (item.IsGuessState(selectedData.Guess))
            {
                item.transform.parent = _cardsActive.transform;
            }
            else
            {
                item.transform.parent = _cardsPassive.transform;
            }
        });
    }

    private void DoFilter(ReactionData reactionData)
    {
        _cardSpreadView.WithAll(item =>
        {
            if (item.HasReaction(reactionData))
            {
                item.transform.parent = _cardsActive.transform;
            }
            else
            {
                item.transform.parent = _cardsPassive.transform;
            }
        });
    }
    
    private void DoFilter(AbilityReactionTriggerData abilityData)
    {
        _cardSpreadView.WithAll(item =>
        {
            if (item.HasAbility(abilityData))
            {
                item.transform.SetParent(_cardsActive.transform,false);
            }
            else
            {
                item.transform.SetParent( _cardsPassive.transform,false);
            }
        });
    }
    
    private void DoFilter(MonsterEntityTypeData monsterEntityTypeData)
    {
        _cardSpreadView.WithAll(item =>
        {
            if (item.HasTrait(monsterEntityTypeData.PreferredTraits))
            {
                item.transform.SetParent(_cardsActive.transform,false);
            }
            else
            {
                item.transform.SetParent( _cardsPassive.transform,false);
            }
        });
    }


    private void HandleClickPrevious()
    {
        CharacterEntity nextCharacter = GetPreviousCharacter(_currentCharacter);
        Set(nextCharacter);
    }

    private void HandleClickNext()
    {
        CharacterEntity nextCharacter = GetNextCharacter(_currentCharacter);
        Set(nextCharacter);
    }

    private CharacterEntity GetNextCharacter(CharacterEntity currentCharacter)
    {
        for (int i = 0; i < _characterEntities.Count; i++)
        {
            if (_characterEntities[i] == currentCharacter)
            {
                if (i < _characterEntities.Count - 1)
                {
                    return _characterEntities[i + 1];
                }
                else
                {
                    return _characterEntities[0];
                }
            }
        }
        return _characterEntities[0];
    }

    private CharacterEntity GetPreviousCharacter(CharacterEntity currentCharacter)
    {
        for (int i = 0; i < _characterEntities.Count; i++)
        {
            if (_characterEntities[i] == currentCharacter)
            {
                if (i > 0)
                {
                    return _characterEntities[i - 1];
                }
                else
                {
                    return _characterEntities[_characterEntities.Count - 1];
                }
            }
        }
        return _characterEntities[_characterEntities.Count - 1];
    }

    public void Set(CharacterEntity characterEntity)
    {
        _currentCharacter = characterEntity;
        if (_cardCharacterView != null)
        {
            _cardCharacterView.Set(characterEntity);
        }
        _cardSpreadView.SetGuessInfo(_currentCharacter.TraitGuessInfo);
        _lastInteractionData = _gameStateContext.LastInteraction;
        if (_lastInteractionData != null)
        {
            if (_lastInteractionData.Reactions.TryGetValue(_currentCharacter, out var reactionData))
            {
                _lastReactionView.Set(reactionData);
                characterEntity.WasSeen.Value = true;
            }
            else
            {
                _lastReactionView.Clear();
            }

            if (_lastInteractionData.Interaction is AbilityReactionTriggerData reactionTriggerData)
            {
                DoFilter(reactionTriggerData);
            }
        }
        else
        {
            _lastReactionView.Clear();
        }
        _guessTypeSelector.Set(_currentCharacter.TypeGuess);
    }

    public void Clear()
    {
        
    }
}
