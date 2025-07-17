using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStarter : MonoBehaviour
{
    [SerializeField] private float _delayRecharge;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _rewardDuration = 20f;
    [SerializeField] private RewardADStarter _rewardPrice;
    [SerializeField] private RewardADStarter _rewardSpeed;
    [SerializeField] private RewardADStarter _rewardCount;

    private List<RewardADStarter> _rewardADStarters;
    private Coroutine _waitActivateNextReward;
    private int _rewardIndex = 0;

    private void Awake()
    {
        _rewardADStarters = new List<RewardADStarter>();

        AddButtonInList(_rewardSpeed);
        AddButtonInList(_rewardPrice);
        AddButtonInList(_rewardCount);
    }

    private void Start()
    {
        _waitActivateNextReward = StartCoroutine(WaitActivateNextReward(_startDelay));
    }

    private void OnEnable()
    {
        _rewardPrice.Switched += OnSwitched;
        _rewardSpeed.Switched += OnSwitched;
        _rewardCount.Switched += OnSwitched;
    }

    private void OnDisable()
    {
        _rewardPrice.Switched -= OnSwitched;
        _rewardSpeed.Switched -= OnSwitched;
        _rewardCount.Switched -= OnSwitched;
    }

    private IEnumerator WaitActivateNextReward(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);
        while (true)
        {
            for (int i = 0; i < _rewardADStarters.Count; i++)
            {
                _rewardADStarters[i].gameObject.SetActive(i == _rewardIndex);
            }
            yield return new WaitForSeconds(_rewardDuration);

            IncreaseRewardIndex();
        }
    }

    private void OnSwitched(RewardADStarter rewardADStarter)
    {
        if (_waitActivateNextReward != null)
            StopCoroutine(_waitActivateNextReward);
        IncreaseRewardIndex();
        _waitActivateNextReward = StartCoroutine(WaitActivateNextReward(65f));
    }

    private void AddButtonInList(RewardADStarter button)
    {
        _rewardADStarters.Add(button);
    }

    private void IncreaseRewardIndex()
    {
        _rewardIndex++;
        if (_rewardIndex >= _rewardADStarters.Count)
            _rewardIndex = 0;
    }
}
