using System;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _mobileInputTutorial;
    [SerializeField] private GameObject _keyboardInputTutorial;

    private int _exitCount = 0;
    private int _maxExitCount = 1;
    private bool _isFirstLaunch = true;

    private void OnEnable()
    {
        WebSdk.ADPlayed += OnADPlayed;
    }

    private void OnDisable()
    {
        WebSdk.ADPlayed -= OnADPlayed;
    }

    private void Start()
    {
        SelectTutorial();
    }

    private void OnADPlayed(bool isPlayed)
    {
        if (isPlayed == true && _isFirstLaunch == true)
            _maxExitCount++;

        if (isPlayed == false && _isFirstLaunch == true)
            SelectTutorial();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _exitCount++;
            if (_exitCount < _maxExitCount)
                return;

            _mobileInputTutorial.SetActive(false);
            _keyboardInputTutorial.SetActive(false);
            _isFirstLaunch = false;
        }
    }

    private void SelectTutorial()
    {
        _mobileInputTutorial.SetActive(Application.isMobilePlatform == true);
        _keyboardInputTutorial.SetActive(Application.isMobilePlatform == false);
    }
}
