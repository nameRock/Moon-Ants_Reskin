using Agava.YandexGames;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _autorizationButton;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Transform _content;
    [SerializeField] private PlayerScoreView _scoreViewPrefab;
    [SerializeField] private WebSdk _webSdk;
    [SerializeField] private Data _data;
    [SerializeField] private TMP_Text _notAutozationText;
    [SerializeField] private GameObject _templateButton;

    private bool _isWaitingRequest = false;
    private float _waitingDelay = 1f;
    private Coroutine _waitSetingsNextScore;
    private List<PlayerScoreView> _scoreViews = new List<PlayerScoreView>();

    private void Awake()
    {
#if !YANDEX_GAMES
        gameObject.SetActive(false);
#endif
    }
#if !UNITY_EDITOR && YANDEX_GAMES
    private void OnEnable()
    {
        _webSdk.LeaderboardGeted += OnLeaderboardGeted;
        _webSdk.PersonalPermissionGeted += OnPersonalPermissionGeted;
        Data.Setted += OnDataSetted;
        _autorizationButton.onClick.AddListener(OnAutorizationButtonClick);

        _openButton.onClick.AddListener(OnOpenButtonClick);
        _closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnDisable()
    {
        Data.Setted -= OnDataSetted;
        _webSdk.Initialized -= OnWebSdkInitialized;
        _webSdk.LeaderboardGeted -= OnLeaderboardGeted;
        _webSdk.TrySetLeaderboardScore(_data.GetCountUpgrade());
        _webSdk.PersonalPermissionGeted -= OnPersonalPermissionGeted;
        _autorizationButton.onClick.AddListener(OnAutorizationButtonClick);
        _openButton.onClick.RemoveListener(OnOpenButtonClick);
        _closeButton.onClick.RemoveListener(OnCloseButtonClick);
    }

    private void OnDataSetted()
    {
        if (PlayerAccount.IsAuthorized == false)
            return;

        if (PlayerAccount.HasPersonalProfileDataPermission == true)
            _webSdk.TrySetLeaderboardScore(_data.GetCountUpgrade());

        _webSdk.TryGetLeaderboardEntries();
    }

    private void OnWebSdkInitialized()
    {
        if (PlayerAccount.IsAuthorized == true)
            _webSdk.TryGetLeaderboardEntries();
    }

    private void OnOpenButtonClick()
    {
        _panel.SetActive(true);

        if (_isWaitingRequest == true)
            return;

        StartCoroutine(WaitRequest());

        if (PlayerAccount.IsAuthorized == false)
            return;

        if (PlayerAccount.HasPersonalProfileDataPermission == true)
            _webSdk.TrySetLeaderboardScore(_data.GetCountUpgrade());

        _notAutozationText.gameObject.SetActive(false);

        if (PlayerAccount.HasPersonalProfileDataPermission == false)
            _webSdk.TryGetPersonalProfileDataPermission();
    }


    private void OnAutorizationButtonClick()
    {
        _webSdk.AuthorizePlayer();
    }

    private void OnPersonalPermissionGeted()
    {
        _webSdk.TrySetLeaderboardScore(_data.GetCountUpgrade());
        _webSdk.TryGetLeaderboardEntries();
    }

    private void OnLeaderboardGeted(LeaderboardGetEntriesResponse response)
    {
        if (response == null)
            return;

        _notAutozationText.gameObject.SetActive(false);

        ClearScoreViews();

        foreach (var entry in response.entries)
        {
            string name = entry.player.publicName;
            if (string.IsNullOrEmpty(name))
                name = "Anonymous";
            PlayerScoreView scoreView = Instantiate(_scoreViewPrefab, _content);
            scoreView.Init(name, entry.score);
            _scoreViews.Add(scoreView);
        }
    }

    private void ClearScoreViews()
    {
        foreach (var view in _scoreViews)
        {
            Destroy(view.gameObject);
        }
        _scoreViews.Clear();
    }

    private IEnumerator WaitSettingNextScore()
    {
        float delay = 5;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
        yield return waitForSeconds;
        _webSdk.TrySetLeaderboardScore(_data.GetCountUpgrade());
    }

    private IEnumerator WaitRequest()
    {
        _isWaitingRequest = true;
        float requestDelay = 5f;
        float entriesDelay = 0.5f;

        yield return new WaitForSeconds(entriesDelay);
        if (PlayerAccount.IsAuthorized == true)
            _webSdk.TryGetLeaderboardEntries();

        yield return new WaitForSeconds(requestDelay);
        _isWaitingRequest = false;
    }
#endif

    public void CloseLiders()
    {
        OnCloseButtonClick();
    }

    public void DisableButton()
    {
        _openButton.gameObject.SetActive(false);
        _templateButton.SetActive(true);
    }

    public void EnableButton()
    {
        _templateButton.SetActive(false);
        _openButton.gameObject.SetActive(true);
    }

    private void OnCloseButtonClick()
    {
        _panel.SetActive(false);
    }
}