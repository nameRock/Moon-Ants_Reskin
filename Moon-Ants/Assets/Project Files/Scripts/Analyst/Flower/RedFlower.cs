using Agava.YandexMetrica;
using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFlower : SenderEvents
{
    private void OnEnable()
    {
        SendEventOffer();
    }

    protected override void SendEventOffer()
    {
#if YANDEX_GAMES && !UNITY_EDITOR
        YandexMetrica.Send(MetricaEventsNameHolder.RedFlowerAdOffer);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("RedFlower-ad-offer");
#endif
    }
    protected override void SendEventClick()
    {
#if YANDEX_GAMES && !UNITY_EDITOR
        YandexMetrica.Send(MetricaEventsNameHolder.RedFlowerAdClick);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("RedFlower-ad-click");
#endif
    }
}
