using Agava.YandexMetrica;
using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBonus : SenderEvents
{
    private void OnEnable()
    {
        SendEventOffer();
    }

    protected override void SendEventOffer()
    {
#if YANDEX_GAMES && !UNITY_EDITOR
        YandexMetrica.Send(MetricaEventsNameHolder.SpeedBonusAdOffer);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("SpeedBonus-ad-offer");
#endif
    }
    protected override void SendEventClick()
    {
#if YANDEX_GAMES && !UNITY_EDITOR
        YandexMetrica.Send(MetricaEventsNameHolder.SpeedBonusAdClick);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("SpeedBonus-ad-click");
#endif
    }
}
