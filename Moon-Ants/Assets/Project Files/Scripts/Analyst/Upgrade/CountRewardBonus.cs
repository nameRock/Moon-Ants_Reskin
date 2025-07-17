using Agava.YandexMetrica;
using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountRewardBonus : SenderEvents
{
    protected override void SendEventOffer()
    {
#if YANDEX_GAMES && !UNITY_EDITOR
        YandexMetrica.Send(MetricaEventsNameHolder.CountUpAdOffer);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("CountRewardBonus-ad-offer");
#endif
    }
    protected override void SendEventClick()
    {
#if YANDEX_GAMES && !UNITY_EDITOR
        YandexMetrica.Send(MetricaEventsNameHolder.CountUpAdClick);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("CountRewardBonus-ad-click");
#endif
    }
}
