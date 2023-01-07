using System.Collections.Generic;
using System.Linq;
using Context;
using Data.Monsters;
using NiftyFramework.Core.Context;
using NiftyFramework.UnityUtils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Guide
{
    public class UIGuideGuestList : MonoBehaviour
    {
        private MonoPool<UIGuideGuestListItem> _itemViewPool;
        [SerializeField] private LayoutGroup _itemLayout;
        //TODO - we should poll from the game state to prevent issues if there are ever other monster lists.
        [SerializeField] private MonsterEntityTypeDataSet _monsterEntityTypeDataSet;
        private void Start()
        {
            var viewItems = _itemLayout.GetComponentsInChildren<UIGuideGuestListItem>();
            _itemViewPool = new MonoPool<UIGuideGuestListItem>(viewItems);
            ContextService.Get<GameStateContext>(gameState =>
            {
                foreach (var characterEntity in gameState.CharacterEntities)
                {
                    if (_itemViewPool.TryGet(out var view))
                    {
                        view.Set(characterEntity, _monsterEntityTypeDataSet);
                    }
                }
            });
        }
    }
}