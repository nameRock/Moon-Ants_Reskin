using UnityEngine;
using System;
using Agava.YandexGames;
using Agava.VKGames;
using CrazyGames;
using System.Collections;
using UnityEngine.SceneManagement;
using Agava.WebUtility;

public class WebSdk : MonoBehaviour
{
    private const string LeaderBoardName = "Leaderboard";

    [SerializeField] private bool _isStartInterstitialAD = true;

    public event Action<string> RewardVideoStarted;
    public event Action<string> RewardVideoCompleted;
    public event Action<string> RewardVideoClosed;
    public event Action<string> RewardVideoErrored;
    public event Action InterstitalVideoCompleted;
    public event Action InterstitalVideoErrored;
    public event Action FriendsInvited;
    public event Action Initialized;
    public event Action PersonalPermissionGeted;
    public event Action AdBlockDetected;
    public event Action<LeaderboardGetEntriesResponse> LeaderboardGeted;

    public static event Action<bool> ADPlayed;

    private int _lastScore;
    private string _requestID;
    private VideoType _videoType = VideoType.Not;


#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize();

        if (_isStartInterstitialAD == true)
            ShowInterstitialAD();
        Initialized?.Invoke();
    }
#endif


#if CRAZY_GAMES ||GAME_DISTRIBUTION && UNITY_WEBGL && !UNITY_EDITOR
    private void Start()
    {
        //Debug.Log("WebSDK Start ShowInterstitialAD()");
        //ShowInterstitialAD();
    }
#endif

    public void ShowRewardedAD(string rewardID, bool detectADBlock)
    {
        Debug.Log("ShowRewardedAD()");

        _requestID = rewardID;
#if YANDEX_GAMES
        if (AdBlock.Enabled == true)
        {
            if (detectADBlock == true)
                AdBlockDetected?.Invoke();

            CompletePause();
            RewardVideoErrored?.Invoke(_requestID);
            return;
        }
        Agava.YandexGames.VideoAd.Show(OnRewardedOpenCallback, OnRewardedCompleteCallback, OnRewardedCloseCallback, OnRewardedErrorCallback);
        RewardVideoStarted?.Invoke(_requestID);
#endif
#if CRAZY_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        RewardVideoStarted?.Invoke(_requestID);
        CrazyAds.Instance.beginAdBreakRewarded(OnVideoStartedCallback, OnRewardedCompleteCallback, OnRewardedErrorCallback);
#endif

#if VK_GAMES
        BeginPause();
        RewardVideoStarted?.Invoke(_requestID);
        Agava.VKGames.VideoAd.Show(OnRewardedCallback, OnErrorCallback);
#endif
#if GAME_DISTRIBUTION
        _videoType = VideoType.Rewarded;
        RewardVideoStarted?.Invoke(_requestID);
        GameDistribution.Instance.ShowRewardedAd();
#endif
    }

    public void ShowInterstitialAD()
    {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        if (AdBlock.Enabled == true)
        {
            CompletePause();
            return;
        }
        InterstitialAd.Show(OnInterstitialOpenCallback, OnInterstitialCloseCallback, OnInterstitialErrorCallback, OnInterstitialOfflineCallback);
#endif
#if VK_GAMES
        VideoAd.Show(OnRewardedCallback, OnErrorCallback);
#endif
#if CRAZY_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        CrazyAds.Instance.beginAdBreak(OnVideoStartedCallback, OnInterstitialCompleted, OnInterstitialErrored);
#endif
#if GAME_DISTRIBUTION
        Debug.Log("ShowInterstitialAD()");

        _videoType = VideoType.Interstital;
        GameDistribution.Instance.ShowAd();
#endif
    }

#if YANDEX_GAMES
    #region YANDEXLeaderboard
    public void TryGetLeaderboardEntries()
    {
        Agava.YandexGames.Leaderboard.GetEntries(LeaderBoardName, OnLeaderboerdGetEntriesSuccesCallback, null, 10, 5);
    }

    private void OnLeaderboerdGetEntriesSuccesCallback(LeaderboardGetEntriesResponse entries)
    {
        LeaderboardGeted?.Invoke(entries);
    }

    public void TrySetLeaderboardScore(int score)
    {
        if (YandexGamesSdk.IsInitialized == true && PlayerAccount.IsAuthorized == true)
        {
            _lastScore = score;
            Agava.YandexGames.Leaderboard.GetPlayerEntry(LeaderBoardName, OnPlayerEntryGeted, OnPlayerEntryErrored);
        }
    }

    private void OnPlayerEntryGeted(LeaderboardEntryResponse player)
    {
        //if(player!= null && player.player.publicName == "DpyrCoToHbl")
        //{
        //    Agava.YandexGames.Leaderboard.SetScore(LeaderBoardName, 66);
        //    return;
        //}
        if (player != null && player.score < _lastScore)
            Agava.YandexGames.Leaderboard.SetScore(LeaderBoardName, _lastScore);
        //if(player == null)
        //    Agava.YandexGames.Leaderboard.SetScore(LeaderBoardName, _lastScore);
    }

    private void OnPlayerEntryErrored(string info)
    {
        Debug.Log("OnPlayerEntryErrored");
    }

    #endregion

    #region YANDEXAuthorizePlayer
    public void AuthorizePlayer()
    {
        if (PlayerAccount.IsAuthorized == false)
            PlayerAccount.Authorize(TryGetPersonalProfileDataPermission);
    }

    public void TryGetPersonalProfileDataPermission()
    {
        if (PlayerAccount.IsAuthorized == true && PlayerAccount.HasPersonalProfileDataPermission == false)
        {
            PlayerAccount.RequestPersonalProfileDataPermission(OnPersonalDataPermission);
        }
    }

    private void OnPersonalDataPermission()
    {
        PersonalPermissionGeted?.Invoke();
    }
    #endregion

    #region YANDEXInterstitialAD
    private void OnInterstitialOpenCallback() => BeginPause();

    private void OnInterstitialCloseCallback(bool obj)
    {
        CompletePause();
    }

    private void OnInterstitialErrorCallback(string obj) => CompletePause();

    private void OnInterstitialOfflineCallback() => CompletePause();
    #endregion


    #region YANDEXRewardedAD
    private void OnRewardedOpenCallback() => BeginPause();


    private void OnRewardedCompleteCallback()
    {
        RewardVideoCompleted?.Invoke(_requestID);
    }

    private void OnRewardedCloseCallback()
    {
        CompletePause();
        RewardVideoClosed?.Invoke(_requestID);
    }

    private void OnRewardedErrorCallback(string obj)
    {
        CompletePause();
        RewardVideoErrored?.Invoke(_requestID);
    }
    #endregion
#endif

    private void BeginPause()
    {
        ADPlayed?.Invoke(true);
    }

    private void CompletePause()
    {
        ADPlayed?.Invoke(false);
    }

#if VK_GAMES && UNITY_WEBGL && !UNITY_EDITOR
    private IEnumerator Start()
    {
        yield return VKGamesSdk.Initialize();
        BeginPause();
        Interstitial.Show(OnInterstitialOpenCallback, OnInterstitialErrorCallback);
    }

    public void SetLeaderboardScore(int score)
    {
        Leaderboard.ShowLeaderboard(score);
    }

    public void InviteFriends()
    {
        SocialInteraction.InviteFriends(OnFriendsInvited);
    }

    private void OnFriendsInvited() => FriendsInvited?.Invoke();   

    private void OnInterstitialOpenCallback() => CompletePause();

    private void OnInterstitialErrorCallback() => CompletePause();

    private void OnRewardedCallback()
    {
        CompletePause();
        RewardVideoCompleted?.Invoke(_requestID);
    }

    private void OnErrorCallback() => CompletePause();
#endif

#if CRAZY_GAMES && UNITY_WEBGL && !UNITY_EDITOR
    private void OnVideoStartedCallback()
    {
        BeginPause();
    }

    private void OnRewardedCompleteCallback()
    {
        CompletePause();
        RewardVideoCompleted?.Invoke(_requestID);
        RewardVideoClosed?.Invoke(_requestID);
    }
    private void OnRewardedErrorCallback()
    {
        CompletePause();
        RewardVideoErrored?.Invoke(_requestID);
    }

    private void OnInterstitialCompleted()
    {
        CompletePause();
        InterstitalVideoCompleted?.Invoke();
    }

    private void OnInterstitialErrored()
    {
        CompletePause();
        InterstitalVideoErrored?.Invoke();
    }
#endif
#if GAME_DISTRIBUTION

    private void OnEnable()
    {
        GameDistribution.OnResumeGame += OnResumeGame;
        GameDistribution.OnPauseGame += OnPauseGame;
        GameDistribution.OnRewardGame += OnRewardedGame;
        GameDistribution.OnRewardedVideoFailure += OnRewardedVideoFailure;
    }

    private void OnDisable()
    {
        GameDistribution.OnResumeGame -= OnResumeGame;
        GameDistribution.OnPauseGame -= OnPauseGame;
        GameDistribution.OnRewardGame -= OnRewardedGame;
        GameDistribution.OnRewardedVideoFailure -= OnRewardedVideoFailure;
    }

    private void OnRewardedVideoFailure()
    {
        CompletePause();
        RewardVideoErrored?.Invoke(_requestID);
    }

    private void OnRewardedGame()
    {
        CompletePause();
        RewardVideoCompleted?.Invoke(_requestID);
        RewardVideoClosed?.Invoke(_requestID);
    }

    private void OnPauseGame()
    {
        BeginPause();
    }

    private void OnResumeGame()
    {
        CompletePause();
        if (_videoType == VideoType.Interstital)
            InterstitalVideoCompleted?.Invoke();
        Debug.Log("OnResumeGame()");
    }

#endif
    private enum VideoType
    {
        Not,
        Interstital,
        Rewarded
    }
}