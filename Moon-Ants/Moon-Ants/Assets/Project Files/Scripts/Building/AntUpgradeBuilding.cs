using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntUpgradeBuilding : MonoBehaviour
{
    private const string IsMove = "isMove";

    [SerializeField] private ButtonPanel _buttonPanel;
    [SerializeField] private Upgrader _countUpgrade;
    [SerializeField] private Upgrader _speedUpgrade;
    [SerializeField] private Upgrader _forceUpgrade;
    [SerializeField] private Animator _circleAnimator;
    [SerializeField] private GameObject _zoneParticls;
    [SerializeField] private InterstitialADTimer _interstitialADTimer;
    [SerializeField] private SkinsDisplay _skinsDisplay;
    [SerializeField] private LeaderboardView _leaderboardView;
    [SerializeField] private GameObject _tutor;

    private bool _enter = false;

    private void Update()
    {
        if (_countUpgrade.IsActive || _speedUpgrade.IsActive || _forceUpgrade.IsActive)
        {
            _circleAnimator.SetBool(IsMove, true);
            _zoneParticls.SetActive(true);
        }
        else
        {
            _circleAnimator.SetBool(IsMove, false);
            _zoneParticls.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (_enter == false)
            {
                _enter = true;
                _skinsDisplay.DisableButton();
                _leaderboardView.DisableButton();
                _interstitialADTimer.TryShowAD(player);

                _buttonPanel.OpenPanel();

                if(_tutor != null)
                {
                    _tutor.SetActive(false);
                }
            }

#if !UNITY_EDITOR && YANDEX_GAMES
            _leaderboardView.CloseLiders();
#endif
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (player.PlayerMover.InputDirectionMagnitude != 0 && _enter == true)
            {
                _enter = false;

                _buttonPanel.ClosePanel();
                _skinsDisplay.EnableButton();
                _leaderboardView.EnableButton();
            }
        }
    }
}
