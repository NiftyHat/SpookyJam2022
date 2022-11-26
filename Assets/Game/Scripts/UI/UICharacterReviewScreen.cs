using System.Collections.Generic;
using System.Linq;
using Context;
using Data;
using Data.Interactions;
using Data.Reactions;
using Data.Trait;
using Entity;
using NiftyFramework.Core.Context;
using NiftyFramework.UI;
using NiftyFramework.UnityUtils;
using UI;
using UI.Cards;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterReviewScreen : MonoBehaviour, IView<CharacterEntity>
{
    [SerializeField] private UICardCharacterView _cardCharacterView;
    [SerializeField] private TraitDataSet _traitData;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private ReactionDataSet _reactionDataSet;

    [SerializeField] private Button _buttonNext;
    [SerializeField] private Button _buttonPrevious;
    [SerializeField] private UICardSpreadView _cardSpreadView;
    [SerializeField] private LayoutGroup _reactionFilterLayout;
    [SerializeField] private Button _clearFilterButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private UIReactionView _lastReactionView;

    [SerializeField] private GameObject _root;
    
    private GameStateContext _gameStateContext;
    private CharacterEntity _currentCharacter;
    private IReadOnlyList<CharacterEntity> _characterEntities;
    private MonoPool<UIFilterButtonView> _filterButtonPool;

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

        var filterButtons = _reactionFilterLayout.GetComponentsInChildren<UIFilterButtonView>();
        _filterButtonPool = new MonoPool<UIFilterButtonView>(filterButtons);
        
        List<AbilityReactionTriggerData> reactionAbilityList = _playerData.GetInteractionDataList<AbilityReactionTriggerData>();

        for (int i = 0; i < reactionAbilityList.Count; i++)
        {
            var ability = reactionAbilityList[i];
            if (_filterButtonPool.TryGet(out var view))
            {
                UIFilterButtonView.Data<AbilityReactionTriggerData> viewData =
                    new UIFilterButtonView.Data<AbilityReactionTriggerData>(ability, ability.MenuItem);
                view.Set(viewData);
                view.OnSelected += HandleSelectedFilter;
            }
        }

        /*
        for (int i = 0; i < _reactionDataSet.References.Count; i++)
        {
            var reactionData = _reactionDataSet.References[i];
            if (reactionData.isMiss == false)
            {
                if (_filterButtonPool.TryGet(out var view))
                {
                    var filter = new UIFilterButtonView.Data<ReactionData>(reactionData);
                    view.Set(filter);
                    view.OnSelected += HandleSelectedFilter;
                }
            }
        }*/

        _cardSpreadView.Set(_traitData.References, _playerData);
        if (_currentCharacter != null)
        {
            _cardSpreadView.SetSelected(_currentCharacter.TraitGuessList);
        }
        
        _clearFilterButton.onClick.AddListener(HandleClickClear);
        
        _cardSpreadView.OnSelectedTraitsChanged += HandleSelectedTraitsChanged;

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
        Set(characterEntity);
       
    }

    private void HandleClickClear()
    {
        _cardSpreadView.ClearSelection();
    }

    private void HandleSelectedTraitsChanged(List<TraitData> traitList)
    {
        _currentCharacter.SetTraitGuess(traitList.ToList());
        _cardCharacterView.SetTraitGuesses(traitList.ToList());
    }

    private void HandleSelectedFilter(UIFilterButtonView.Data buttonData)
    {
        if (buttonData is UIFilterButtonView.Data<ReactionData> reactionButton)
        {
            DoFilter(reactionButton.Item);
        }
        if (buttonData is UIFilterButtonView.Data<AbilityReactionTriggerData> abilityButton)
        {
            DoFilter(abilityButton.Item);
        }
    }

    private void DoFilter(ReactionData reactionData)
    {
        _cardSpreadView.WithAll(item =>
        {
            if (item.HasReaction(reactionData))
            {
                item.SetFacingDown(false);
            }
            else
            {
                item.SetFacingDown(true);
            }
        });
    }
    
    private void DoFilter(AbilityReactionTriggerData abilityData)
    {
        _cardSpreadView.WithAll(item =>
        {
            if (item.HasAbility(abilityData))
            {
                item.SetFacingDown(false);
            }
            else
            {
                item.SetFacingDown(true);
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
            _cardCharacterView.SetFacingDown(false);
            _cardCharacterView.Set(characterEntity);
            
        }
        _cardSpreadView.SetSelected(_currentCharacter.TraitGuessList);
        if (characterEntity.LastReaction != null)
        {
            _lastReactionView.Set(characterEntity.LastReaction);
            //DoFilter(characterEntity.LastReaction);
        }
        else
        {
            _lastReactionView.Clear();
        }
    }

    public void Clear()
    {
        _cardCharacterView.SetFacingDown(true);
        //throw new System.NotImplementedException();
    }
}
