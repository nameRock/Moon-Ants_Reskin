using UnityEngine;

namespace CrazyGames
{
    class CrazySDKInit
    {
        #if CRAZY_GAMES

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnRuntimeMethodLoad()
        {
            CrazySDK.ResetDomain();
            CrazyAds.ResetDomain();
            CrazyEvents.ResetDomain();
            
            var sdk = CrazySDK.Instance; // Trigger init by calling instance
        }
#endif
    }
}