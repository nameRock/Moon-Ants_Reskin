using GameAnalyticsSDK;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreensaverSceneManager : MonoBehaviour
{
    [SerializeField] private bool _isRemoveDataOnStart = false;
    [SerializeField] private Data _data;
    [SerializeField] private WebSdk _webSdk;

    private Saver _saver;

    private void Awake()
    {
        _saver = _data.GetComponent<Saver>();
    }
#if YANDEX_GAMES
    private void OnEnable()
    {
        _webSdk.Initialized += Initialized;
    }

    private void OnDisable()
    {
        _webSdk.Initialized -= Initialized;
    }
#endif

#if CRAZY_GAMES||GAME_DISTRIBUTION
    private void Start()
    {
        Initialized();
    }
#endif
    private void Initialized()
    {
        Debug.Log("ScreensaverSceneManager Initialized");
        GameAnalytics.Initialize();

        _data.AddSession();
        _data.SetLastLoginDate(DateTime.Now);
        _data.SetInterstitialADDelay(120);
        _saver.Loaded += OnDataLoaded;
        _saver.Load();

        Debug.Log("ScreensaverSceneManager Initialized End");
    }

    private void OnDataLoaded()
    {
        Debug.Log("ScreensaverSceneManager OnDataLoaded()");

        SaveData();
    }

    private void SaveData()
    {
        Debug.Log("ScreensaverSceneManager SaveData()");

        _saver.Saved += LoadScene;
#if CRAZY_GAMES
        _data.SetIgnoreNextInterstialAd(true);
#else
        _data.SetIgnoreNextInterstialAd(false);
#endif
        _saver.Save();
    }

    private void LoadScene()
    {
        Debug.Log("ScreensaverSceneManager LoadScene()");
        _saver.Saved -= LoadScene;
        SceneManager.LoadScene(_data.GetLevelIndex());
    }
}
