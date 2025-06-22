using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardsADSwitcher : MonoBehaviour
{
    [SerializeField] private RewardTriggerStarter[] _rewardAdStarters;
    [SerializeField] private WebSdk _webSdk;
    [SerializeField] private Player _player;
    [SerializeField] private float _switchDelay = 60f;
    [SerializeField] private RewardFlowerDisplay _flowerDisplay;

    private int _rewardIndex = 0;
    private bool _isRewardsAvailable => _availableRewardStarters.Count() > 0;
    private IEnumerable<RewardTriggerStarter> _availableRewardStarters => _rewardAdStarters.Where(reward => reward.IsReceived == false);
    private Coroutine _waitActivateNextReward;

    private void Awake()
    {
        foreach (var starter in _rewardAdStarters)
        {
            starter.Init(_webSdk, _player);
            starter.SetRewardDisplay(_flowerDisplay);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < _rewardAdStarters.Length; i++)
            _rewardAdStarters[i].Showed += OnShowNextAd;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _rewardAdStarters.Length; i++)
            _rewardAdStarters[i].Showed -= OnShowNextAd;
    }

    private void Start()
    {
        _waitActivateNextReward = StartCoroutine(WaitActvateNextReward(0));
    }

    private void OnShowNextAd()
    {
        if (_waitActivateNextReward == null)
            _waitActivateNextReward = StartCoroutine(WaitActvateNextReward(_switchDelay));
    }

    private void TryActivateReward(int index)
    {
        RewardTriggerStarter[] starters = _availableRewardStarters.ToArray();
        for (int i = 0; i < starters.Length; i++)
        {
            starters[i].gameObject.SetActive(i == index);
        }
    }

    //private IEnumerator WaitActvateNextReward()
    //{
    //    WaitForSeconds waitForSeconds = new WaitForSeconds(_switchDelay);

    //    while (_isRewardsAvailable == true)
    //    {
    //        int rewardCount = _availableRewardStarters.Count();

    //        if (_rewardIndex >= rewardCount)
    //            _rewardIndex = 0;
    //        TryActivateReward(_rewardIndex);
    //        _rewardIndex++;

    //        yield return waitForSeconds;
    //    }
    //}

    private IEnumerator WaitActvateNextReward(float dilay)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(dilay);

        while (_isRewardsAvailable == true)
        {
            yield return waitForSeconds;

            int rewardCount = _availableRewardStarters.Count();

            if (_rewardIndex >= rewardCount)
                _rewardIndex = 0;
            TryActivateReward(_rewardIndex);
            _rewardIndex++;

            waitForSeconds = new WaitForSeconds(_switchDelay);
        }
        StopCoroutine(_waitActivateNextReward);
        _waitActivateNextReward = null;
    }
}
