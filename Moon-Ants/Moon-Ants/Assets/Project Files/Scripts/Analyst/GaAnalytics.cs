using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaAnalytics : MonoBehaviour
{
    private const string SdkName = "yandexgames";
    
    public void StartLevel(int level)
    {
#if !UNITY_EDITOR
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, level.ToString());
#endif
    }

    public void CompliteLevel(int level)
    {
#if !UNITY_EDITOR
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, level.ToString());
#endif
    }

    public void SourceValue(string nameValue, string source, string chest, float value)
    {
#if !UNITY_EDITOR
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, nameValue, value, source, chest);
#endif
    }

    public void SinkValue(string nameValue, string shop, string shotgun, float value)
    {
#if !UNITY_EDITOR
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, nameValue, value, shop, shotgun);
#endif
    }

    public void ClickedAdReward(string rewardName)
    {
#if !UNITY_EDITOR
        GameAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.Video, SdkName, rewardName);
#endif
    }
    public void ShowAdReward(string rewardName)
    {
#if !UNITY_EDITOR
        GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Video, SdkName, rewardName);
#endif
    }

    public void FailedShowAdReward(string rewardName)
    {
#if !UNITY_EDITOR
        GameAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.Video, SdkName, rewardName);
#endif
    }

    public void RewardReceivedReward(string rewardName)
    {
#if !UNITY_EDITOR
        GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, SdkName, rewardName);
#endif
    }

    public void ShowInter()
    {
#if !UNITY_EDITOR
        GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Interstitial, SdkName, "ShowInter");
#endif
    }

    public void FailedShowInter()
    {
#if !UNITY_EDITOR
        GameAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.Interstitial, SdkName, "FailedShowInter");
#endif
    }

    public void OfferReward(string rewardName)
    {
#if !UNITY_EDITOR
        GameAnalytics.NewAdEvent(GAAdAction.Loaded, GAAdType.Video, SdkName, rewardName);
#endif
    }

    public void ClickReward(string rewardName)
    {
#if !UNITY_EDITOR
        GameAnalytics.NewAdEvent(GAAdAction.Request, GAAdType.Video, SdkName, rewardName);
#endif
    }
}
