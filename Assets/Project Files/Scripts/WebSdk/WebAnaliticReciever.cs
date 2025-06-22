using UnityEngine;

[RequireComponent(typeof(WebSdk))]
public class WebAnaliticReciever : MonoBehaviour
{
    [SerializeField] private GaAnalytics _gaAnalytics;

    private WebSdk _webSdk;

    private void Awake()
    {
        _webSdk = GetComponent<WebSdk>();
    }

    private void OnEnable()
    {
        _webSdk.RewardVideoStarted += OnRewardStarted;
        _webSdk.RewardVideoCompleted += OnRewardCompleted;
        _webSdk.RewardVideoErrored += OnRewardErrored;
        _webSdk.InterstitalVideoCompleted += OnInterstitialVideoCompleted;
        _webSdk.InterstitalVideoErrored += OnInterstitialVideoErrored;
    }

    private void OnDisable()
    {
        _webSdk.RewardVideoStarted -= OnRewardStarted;
        _webSdk.RewardVideoCompleted -= OnRewardCompleted;
        _webSdk.RewardVideoErrored -= OnRewardErrored;
        _webSdk.InterstitalVideoCompleted -= OnInterstitialVideoCompleted;
        _webSdk.InterstitalVideoErrored -= OnInterstitialVideoErrored;
    }

    private void OnRewardErrored(string rewardRequest)
    {
        _gaAnalytics.FailedShowAdReward(GetRewardName(rewardRequest));
    }

    private void OnRewardStarted(string rewardRequest)
    {     
       _gaAnalytics.ClickedAdReward(GetRewardName(rewardRequest));
    }

    private void OnRewardCompleted(string rewardRequest)
    {
        _gaAnalytics.RewardReceivedReward(GetRewardName(rewardRequest));
    }

    private void OnInterstitialVideoCompleted()
    {
        _gaAnalytics.ShowInter();
    }

    private void OnInterstitialVideoErrored()
    {
        _gaAnalytics.FailedShowInter();
    }

    private string GetRewardName(string rewardRequest)
    {
        return rewardRequest.Split(Reward.IDSeparator)[0];
    }
}
