using Context;
using Data;
using NiftyFramework.Core;
using NiftyFramework.Core.Context;
using NiftyFramework.Services;

namespace NiftyFramework
{
    public class AppGame : App
    {
        private StateMachine _stateMachine;
        private ContextService _contextService;
        private AddressableIndex<AssetIndex> _assetIndex;

        public class GameAssetService : AddressableIndex<AssetIndex>
        {
            public GameAssetService(Config config) : base(config)
            {
            }

            public GameAssetService(string indexPath) : base(indexPath)
            {
            }
        }

        public override void Init()
        {
            _stateMachine = new StateMachine();
            _contextService = new ContextService();
            _assetIndex = new GameAssetService("Assets/Game/RuntimeAssets/Data/AssetIndex.asset");
            Services.Register(_assetIndex);
            Services.Register<UpdateService>();
            Services.Register(_contextService);
            
            _assetIndex.Init(() =>
            {
                var timeData = _assetIndex.Get<TimeData>();
                GameStateContext gameStateContext = new GameStateContext(timeData);
                _contextService.Register(gameStateContext);
            });

            base.Init();
            //
        }
    }
}