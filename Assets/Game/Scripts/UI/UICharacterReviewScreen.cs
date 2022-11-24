using System.Collections.Generic;
using System.Linq;
using Context;
using Data;
using Data.Reactions;
using Data.Trait;
using Entity;
using NiftyFramework.Core.Context;
using NiftyFramework.UI;
using NiftyFramework.UnityUtils;
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

    [SerializeField] private GameObject _root;
    
    private GameStateContext _gameStateContext;
    private CharacterEntity _currentCharacter;
    private IReadOnlyList<CharacterEntity> _characterEntities;
    private MonoPool<UIButtonReactionView> _reactionFilterButtonPool;

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

        var reactionButtons = _reactionFilterLayout.GetComponentsInChildren<UIButtonReactionView>();
        _reactionFilterButtonPool = new MonoPool<UIButtonReactionView>(reactionButtons);

        for (int i = 0; i < _reactionDataSet.References.Count; i++)
        {
            var item = _reactionDataSet.References[i];
            if (item.isMiss == false)
            {
                if (_reactionFilterButtonPool.TryGet(out var view))
                {
                    view.Set(item);
                    view.OnSelected += HandleSelectedReaction;
                }
            }
        }

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

    private void HandleSelectedReaction(ReactionData reactionData)
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
            HandleSelectedReaction(characterEntity.LastReaction);
        }
    }

    public void Clear()
    {
        _cardCharacterView.SetFacingDown(true);
        //throw new System.NotImplementedException();
    }
}
