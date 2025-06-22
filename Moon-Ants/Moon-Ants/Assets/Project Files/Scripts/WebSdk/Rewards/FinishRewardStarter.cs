using Agava.YandexMetrica;
using GameAnalyticsSDK;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class FinishRewardStarter : RewardADStarter
{
    [SerializeField] private TMP_Text _multiplierText;
    [SerializeField] private ArrowMultiple _arrowMultiple;
    [SerializeField] private GameSceneManager _sceneManager;
    [SerializeField] private RewardEffectCanvas _rewardEffects;
    [SerializeField] private CanvasGroup[] _hudedGroups;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _multipleRewardButton;
    [SerializeField] private Transform _effectTransform;

    private int _lastMultiplier = 1;

    public event Action RewardSended;
    private bool _rewardReady = false;
    private bool _isWaitRewardEffect = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        _arrowMultiple.Multiplied += OnMultiplied;

#if YANDEX_GAMES && !UNITY_EDITOR
        YandexMetrica.Send("level" + SceneManager.GetActiveScene().buildIndex + "Complete");
        YandexMetrica.Send(MetricaEventsNameHolder.IncreaseEndLevelRewardAdOffer);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("EndLevelRewardAdOffer-ad-offer");
#endif
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _arrowMultiple.Multiplied -= OnMultiplied;
    }

    // Call from Inspector(Button.OnClick)
    public void SendBazeReward()
    {
        if (_isWaitRewardEffect == true)
            return;

        _isWaitRewardEffect = true;
        float effectDelay = 0.3f;
        foreach (var image in _hudedGroups)
        {
            image.DOFade(0, effectDelay);
        }
        StartCoroutine(WaitRewardEffectPlay(effectDelay));
    }

    private IEnumerator WaitRewardEffectPlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _rewardEffects.Completed += OnBazeRewardCompleted;
        _rewardEffects.Play(Reward, false, _nextLevelButton.transform);
    }

    private void OnBazeRewardCompleted()
    {
        _rewardEffects.Completed -= OnBazeRewardCompleted;
        SendReward("Base");
        _sceneManager.LoadNextScene();
    }

    // Call from Inspector(Button.OnClick)
    public void ShowMultipliRewardAd()
    {
#if YANDEX_GAMES && !UNITY_EDITOR
        YandexMetrica.Send(MetricaEventsNameHolder.IncreaseEndLevelRewardAdCklick);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("EndLevelRewardAdOffer-ad-click");
#endif
        ShowRewardAd(false);
    }

    protected override void OnRewardVideoClosed(string requestID)
    {
        if (IsTargetRequest(requestID) == false)
            return;

        if (_rewardReady == false)
        {
            _sceneManager.LoadNextScene();
            return;
        }

        _rewardEffects.Completed += OnRewardEffectCompleted;
        _rewardEffects.Play(Reward, false, _multipleRewardButton.transform);
    }

    private void OnRewardEffectCompleted()
    {
        _rewardEffects.Completed -= OnRewardEffectCompleted;
        SendReward("Reward");
        _sceneManager.LoadNextScene();
    }

    protected override void OnRewardVideoErrored(string requestID)
    {
        if (IsTargetRequest(requestID))
        {
            _rewardEffects.Completed -= OnRewardEffectCompleted;
            SendReward("Base");
            _sceneManager.LoadNextScene();
        }
    }

    protected override void OnRewardedVideoComleted(string requestID)
    {
        if (IsTargetRequest(requestID) == false)
            return;

        MultiplyReward(_lastMultiplier);
        _rewardReady = true;
    }

    private void OnMultiplied(int multiplier)
    {
        _lastMultiplier = multiplier;
        _multiplierText.text = (multiplier * RewardCount).ToString();
    }
}