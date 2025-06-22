using System;
using UnityEngine;

public class RewardTriggerStarter : RewardADStarter
{
    [SerializeField] private SenderEvents _senderEvents;
    [SerializeField] private RewardFlowerDisplay _rewardDisplay;
    [SerializeField] private RewardEffectCanvas _rewardEffects;

    protected override void OnEnable()
    {
        base.OnEnable();
        _rewardDisplay.Clicked += ShowAD;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_rewardDisplay != null)
            _rewardDisplay.Hide();
        _rewardDisplay.Clicked -= ShowAD;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsReceived == true)
            return;  
        if (other.gameObject.TryGetComponent(out Player player))
        {
#if CRAZY_GAMES||GAME_DISTRIBUTION
            _rewardDisplay.Show(Reward);
            return;
#endif
            //ShowAD();
        }
    }

#if CRAZY_GAMES||GAME_DISTRIBUTION
    private void OnTriggerExit(Collider other)
    {
        if (IsReceived == true)
            return;

        if (other.gameObject.TryGetComponent(out Player player))
        {
            _rewardDisplay.Hide();
            return;
        }
    }
#endif

    public void SetRewardDisplay(RewardFlowerDisplay rewardFlowerDisplay)
    {
        _rewardDisplay = rewardFlowerDisplay;
    }

    private void ShowAD()
    {
        ShowRewardAd();
        _senderEvents.SendClick();
    }

    protected override void OnRewardedVideoComleted(string requestID)
    {
        if (IsTargetRequest(requestID) == false)
            return;

        IsReceived = true;
        _rewardEffects.Completed += OnRewardEffectCompleted;
        _rewardEffects.Play(Reward,true, transform);
    }

    private void OnRewardEffectCompleted()
    {
        _rewardEffects.Completed -= OnRewardEffectCompleted;
        SendReward("Reward");
    }
}
