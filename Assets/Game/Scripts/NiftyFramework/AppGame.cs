using Context;
using Data;
using Data.Location;
using Generators;
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
                var guestListGenerator = _assetIndex.Get<GuestListGenerator>();
                var areaDataSet = _assetIndex.Get<LocationDataSet>();
                GameStateContext gameStateContext = new GameStateContext(timeData, guestListGenerator, areaDataSet);
                _contextService.Register<TooltipContext>();
                _contextService.Register(gameStateContext);
            });

            base.Init();
            //
        }
    }
}