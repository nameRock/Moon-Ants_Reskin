using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpgraderADStarter : RewardADStarter
{
    [SerializeField] private Upgrader _upgrader;
    [SerializeField] private Button _rewardButton;
    [SerializeField] private GameObject[] _deactivationsIcons;
    [SerializeField] private SenderEvents _senderEvents;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private float _startDelay = 30f;

    private bool _lastAsctiveState = false;
    private bool _isWaitNextClick = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        _rewardButton.onClick.AddListener(OnRewardButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _rewardButton.onClick.RemoveListener(OnRewardButtonClick);
    }

    private void OnRewardButtonClick()
    {
        if (_senderEvents != null)
        {
            _senderEvents.SendClick();
        }

        ShowRewardAd();
        SwitchState(true);
        _lastAsctiveState = true;
        StartCoroutine(WaitReadyClick());
    }

    private IEnumerator Start()
    {
        _lastAsctiveState = true;        
        SwitchState(_lastAsctiveState);
        enabled = false;
        yield return new WaitForSeconds(_startDelay);
        enabled = true;
    }

    private void Update()
    {
        if (_isWaitNextClick == true)
        {
            return;
        }
        bool isActive = _upgrader.CurrentPrice < _wallet.BlueCountValue;

        if (_lastAsctiveState != isActive)
        {
            SwitchState(isActive);
            _lastAsctiveState = isActive;
        }
    }

    private IEnumerator WaitReadyClick()
    {
        _isWaitNextClick = true;
        yield return new WaitForSeconds(5f);
        _isWaitNextClick = false;
    }
    protected override void DoFinishAction()
    {
        //enabled = false;
        //SwitchState(true);
    }

    private void SwitchState(bool active)
    {
        _rewardButton.gameObject.SetActive(!active);
        foreach (var icon in _deactivationsIcons)
        {
            icon.SetActive(active);
        }

        if (active && _senderEvents != null)
        {
            _senderEvents.SendOffer();
        }
    }
}
