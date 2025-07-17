using Agava.YandexMetrica;
using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigRewardBonus : SenderEvents
{
    protected override void SendEventOffer()
    {
#if YANDEX_GAMES && !UNITY_EDITOR
        YandexMetrica.Send(MetricaEventsNameHolder.DigUpAdOffer);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("DigRewardBonus-ad-offer");
#endif
    }
    protected override void SendEventClick()
    {
#if YANDEX_GAMES && !UNITY_EDITOR
        YandexMetrica.Send(MetricaEventsNameHolder.DigUpAdClick);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("DigRewardBonus-ad-click");
#endif
    }
}
