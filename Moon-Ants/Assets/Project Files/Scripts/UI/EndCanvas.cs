using CrazyGames;
using System.Collections;
using UnityEngine;

public class EndCanvas : MonoBehaviour
{
    [SerializeField] private float _delay = 3;
    [SerializeField] private GameObject _buttonNext;
    [SerializeField] private GameObject _banner;
    [SerializeField] private GameObject _text;
    [SerializeField] private GameObject _blur;
    [SerializeField] private float _delayText;

    private void OnEnable()
    {
#if CRAZY_GAMES
        _text.SetActive(true);
        //_banner.SetActive(true);
        StartCoroutine(StartTimerText());
#endif
    }

    private void OnDisable()
    {
        //if (_banner != null && _banner.activeSelf == true)
        //{
        //    Destroy(_banner);
        //    //CrazyAds.Instance.unregisterBanner(_banner.GetComponent<CrazyBanner>());
        //}
    }

    public void ActivationTimer()
    {
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
#if CRAZY_GAMES
        _delay = 1;
#endif
        yield return new WaitForSeconds(_delay);
        _buttonNext.SetActive(true);
    }

    private IEnumerator StartTimerText()
    {
        yield return new WaitForSeconds(_delayText);
        _blur.SetActive(true);
        _buttonNext.SetActive(true);
    }
}
