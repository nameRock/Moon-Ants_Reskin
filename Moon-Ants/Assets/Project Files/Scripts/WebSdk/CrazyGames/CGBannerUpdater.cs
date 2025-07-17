using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrazyGames;
using System;

public class CGBannerUpdater : MonoBehaviour
{

    [SerializeField] private CrazyBanner _banner;
    [SerializeField] private float _updateDelay = 60f;

    private Coroutine _waitUpdateBanner;
#if CRAZY_GAMES

    private void Awake()
    {
        CrazyAds.Instance.listenToBannerError(OnBannerError);
        CrazyAds.Instance.listenToBannerRendered(OnBannerRendered);
        _banner.MarkVisible(true);
    }

    private void Start()
    {
        if(_waitUpdateBanner != null)
        {
            StopCoroutine(_waitUpdateBanner);
        }
        _waitUpdateBanner = StartCoroutine(WaitUpdateBanner());
  
    }

    private void OnBannerRendered(string id)
    {
        Debug.Log("Banner rendered for id " + id);
    }

    private void OnBannerError(string id, string error)
    {
        Debug.Log("Banner error for id " + id + ": " + error);
    }

    private IEnumerator WaitUpdateBanner()
    {

        while (true)
        {
            CrazyAds.Instance.updateBannersDisplay();
            yield return new WaitForSeconds(_updateDelay);
        }
    }
#endif
}
