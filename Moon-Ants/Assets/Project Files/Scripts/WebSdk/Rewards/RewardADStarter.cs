using System;
using TMPro;
using UnityEngine;

public abstract class RewardADStarter : MonoBehaviour
{
    [SerializeField] private TMP_Text _rewardCountText;
    [SerializeField] protected WebSdk _webSdk;
    [SerializeField] private Player _player;
    [SerializeField] private Reward _reward;
    [SerializeField] protected SizeChangedAnimation _sizeChangedAnimation;
    [SerializeField] private GameObject _icon;

    protected int RewardCount => _reward.RewardValue;
    protected Reward Reward => _reward;
    
    public bool IsReceived { get; protected set; } = false;

    public event Action Received;
    public event Action<RewardADStarter> Switched;
    public event Action Showed;

    private void Awake()
    {
        _reward.GenerateID();
        if (_rewardCountText != null)
            _rewardCountText.text = _reward.Description;
    }

    protected virtual void OnEnable()
    {
        SubscribeWebSdk();
        if (_sizeChangedAnimation != null)
            _sizeChangedAnimation.Play();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeWebSdk();
    }

    public void Init(WebSdk webSdk, Player player)
    {
        _webSdk = webSdk;
        _player = player;
    }

    protected void ShowRewardAd(bool detectADBlock = true)
    {
        _webSdk.ShowRewardedAD(_reward.RewardID, detectADBlock);
        /*
        SendReward();
        Switched?.Invoke(this);

        if(_icon != null)
        {
            _icon.SetActive(true);
        }

        gameObject.SetActive(false);
        */
    }

    protected void SendReward(string Source)
    {
        _reward.Send(_player, "Reward");

        Switched?.Invoke(this);

        if (_icon != null)
        {
            _icon.SetActive(true);
        }
    }

    protected virtual void DoFinishAction()
    {
        gameObject.SetActive(false);
        Showed?.Invoke();
    }

    protected void MultiplyReward(int multiplier)
    {
        _reward.Multiply(multiplier);
    }

    protected bool IsTargetRequest(string requestID)
    {
        return requestID == _reward.RewardID;
    }

    protected virtual void OnRewardVideoErrored(string requestID)
    {
    }

    protected virtual void OnRewardVideoClosed(string requestID)
    {
        if (IsTargetRequest(requestID) == false)
            return;

        DoFinishAction();
    }

    protected void SubscribeWebSdk()
    {
        _webSdk.RewardVideoCompleted += OnRewardedVideoComleted;
        _webSdk.RewardVideoClosed += OnRewardVideoClosed;
        _webSdk.RewardVideoErrored += OnRewardVideoErrored;
    }

    protected void UnsubscribeWebSdk()
    {
        _webSdk.RewardVideoCompleted -= OnRewardedVideoComleted;
        _webSdk.RewardVideoClosed -= OnRewardVideoClosed;
        _webSdk.RewardVideoErrored -= OnRewardVideoErrored;
    }

    protected void TestCallBack()
    {
        OnRewardedVideoComleted(_reward.RewardID);
    }

    protected virtual void OnRewardedVideoComleted(string requestID)
    {
        if (IsTargetRequest(requestID) == false)
            return;

        SendReward("Reward");
        IsReceived = true;
    }
}