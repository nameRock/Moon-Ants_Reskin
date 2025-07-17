using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrader : MonoBehaviour
{
    private const string IsMove = "isMove";

    [SerializeField] private ButtonPanel _buttonPanel;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private PlayerBaza _baza;
    [SerializeField] private PlayerCollector _playerCollector;
    [SerializeField] private ParticleSystem _buffParticl;
    [SerializeField] private Data _data;
    [SerializeField] private Upgrader _capacityUpgrade;
    [SerializeField] private Upgrader _speedUpgrade;
    [SerializeField] private Upgrader _priceUpgrade;
    [SerializeField] private GameObject _zoneParticls;
    [SerializeField] private Animator _circleAnimator;
    [SerializeField] private InterstitialADTimer _interstitialADTimer;
    [SerializeField] private SkinsDisplay _skinsDisplay;
    [SerializeField] private LeaderboardView _leaderboardView;

    private void Start()
    {
        if (Data.IsSeted == true)
        {
            SetSavedData();
            return;
        }
        Data.Setted += OnDataSeted;
    }

    private void OnDataSeted()
    {
        SetSavedData();
        Data.Setted -= OnDataSeted;
    }

    private void SetSavedData()
    {
        _playerCollector.ChangedLengthStack(_data.GetPlayerStackUpgrade());
        _baza.ChagedMultiplier(_data.GetPlayerPriceUpgrade());
        _playerMover.ChangedSpeed(_data.GetPlayerSpeedUpgrade());
    }

    private void Update()
    {
        if (_capacityUpgrade.IsActive || _speedUpgrade.IsActive || _priceUpgrade.IsActive)
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
        if (other.gameObject.TryGetComponent(out Player player))
        {
            _skinsDisplay.DisableButton();

            _buttonPanel.OpenPanel();

            _leaderboardView.DisableButton();
            _interstitialADTimer.TryShowAD(player);

#if !UNITY_EDITOR
            _leaderboardView.CloseLiders();
#endif
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            _buttonPanel.ClosePanel();
            _skinsDisplay.EnableButton();

            _leaderboardView.EnableButton();
        }
    }

    public void UpgradeSpeed()
    {
        _playerMover.ChangedSpeed(_data.GetPlayerSpeedUpgrade());
        _buffParticl.Play();
    }

    public void UpgradePrice()
    {
        _baza.ChagedMultiplier(_data.GetPlayerPriceUpgrade());
        _buffParticl.Play();
    }

    public void UpgradeCapacity()
    {
        _playerCollector.UpgradeLengthStack();
        _buffParticl.Play();
    }
}
