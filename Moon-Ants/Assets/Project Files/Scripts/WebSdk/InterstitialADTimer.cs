using System;
using System.Collections;
using UnityEngine;

public class InterstitialADTimer : MonoBehaviour
{
    [SerializeField] private WebSdk _webSdk;
    [SerializeField] private Data _data;
    [SerializeField] private int _maxDelay = 120;
    [SerializeField] private int _minDelay = 80;
    [SerializeField] private int _delayStep = 5;

    private int _currentDelay;
    private bool _isReady = false;
    private Coroutine _waitShowNextIntersitialAD;

    private void Start()
    {
        if (Data.IsSeted == true)
        {
            OnDataSetted();
            return;
        }
        Data.Setted += OnDataSeted;
    }

    private void OnDataSeted()
    {
        Data.Setted -= OnDataSeted;
        OnDataSetted();
    }

    private void OnDataSetted()
    {
        StartCoroutine(TryShowFirstInterstitialAD());
        if (_waitShowNextIntersitialAD != null)
        {
            StopCoroutine(_waitShowNextIntersitialAD);
        }
        _waitShowNextIntersitialAD = StartCoroutine(WaitShowNextIntersitialAD());
    }
    private void OnEnable()
    {
        _webSdk.RewardVideoStarted += OnRewardVideoStarted;
    }

    private void OnDisable()
    {
        _webSdk.RewardVideoStarted -= OnRewardVideoStarted;
    }

    private IEnumerator TryShowFirstInterstitialAD()
    {
        yield return null;
        if (_data.GetIsIgnoreNextInterstitialAd() == true)
            _data.SetIgnoreNextInterstialAd(false);
        else
            _webSdk.ShowInterstitialAD();
    }

    private IEnumerator WaitShowNextIntersitialAD()
    {
        _currentDelay = Math.Clamp(_data.GetInterstitialADDelay(), _minDelay, _maxDelay);

        while (true)
        {
            yield return new WaitForSeconds(_currentDelay);
            _currentDelay -= _delayStep;
            _currentDelay = Math.Clamp(_currentDelay, _minDelay, _maxDelay);
            _data.SetInterstitialADDelay(_currentDelay);
            _data.SetIgnoreNextInterstialAd(false);
            _isReady = true;
        }
    }
    private void OnRewardVideoStarted(string obj)
    {
#if CRAZY_GAMES 
        return;
#endif
        _data.SetIgnoreNextInterstialAd(true);

        _isReady = false;
        if (_waitShowNextIntersitialAD != null)
        {
            StopCoroutine(_waitShowNextIntersitialAD);
        }
        _currentDelay = _maxDelay;
        _data.SetInterstitialADDelay(_currentDelay);
        _waitShowNextIntersitialAD = StartCoroutine(WaitShowNextIntersitialAD());
    }

    public void TryShowAD(Player player)
    {
#if  CRAZY_GAMES|| GAME_DISTRIBUTION
        return;
#endif

        if (_isReady == true)
        {
#if !UNITY_EDITOR && CRAZY_GAMES
            player.PlayerMover.SetState(false);
#endif
            _webSdk.ShowInterstitialAD();
            _isReady = false;
        }
    }
}
