using Context;
using Entity;
using NiftyFramework.Core.Context;
using UnityEngine;

namespace UI.Widgets
{
    public class ScandalBarWidget : MonoBehaviour
    {
        [SerializeField] private GameStatSegmentedBarWidget _segmentedBar;

        protected void Start()
        {
            ContextService.Get<GameStateContext>(HandleGameState);
        }

        private void HandleGameState(GameStateContext service)
        {
            service.GetPlayer(HandlePlayer);
        }

        private void HandlePlayer(PlayerInputHandler player)
        {
            _segmentedBar.Set(player.Stats.Scandal);
        }
    }
}