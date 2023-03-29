using UnityEngine;

namespace NiftyFramework
{
    public static class NiftyBootstrap
    {
        static AppGame _app;
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            _app = new AppGame();
            Debug.Log("Bootstrap worked!");
            _app.Init();
        }

        public static void Reset()
        {
            _app.Reset();
        }
    }
}