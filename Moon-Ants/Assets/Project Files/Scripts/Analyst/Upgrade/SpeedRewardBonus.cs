using Agava.YandexMetrica;
using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedRewardBonus : SenderEvents
{
    protected override void SendEventOffer()
    {
#if YANDEX_GAMES && !UNITY_EDITOR
        YandexMetrica.Send(MetricaEventsNameHolder.SpeedUpAdOffer);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("SpeedRewardBonus-ad-offer");
#endif
    }
    protected override void SendEventClick()
    {
#if YANDEX_GAMES && !UNITY_EDITOR
        YandexMetrica.Send(MetricaEventsNameHolder.SpeedUpAdClick);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("SpeedRewardBonus-ad-click");
#endif
    }
}
