using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntParametersSetter : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float _startSpeed = 2f;
    [SerializeField] private float _speedStep = 0.1f;
    [Header("Digging")]
    [SerializeField] private float _startDigDelay = 2f;
    [SerializeField] private float _digReducingCoefficient = 1f;
    [SerializeField] private float _digReducingStep = 0.1f;

    private float _currentSpeed;
    private float _currentReducingCoefficient;
    private float _speedBoostCoefficient = 1f;
    private float _totalSpeed => _currentSpeed * _speedBoostCoefficient;

    public Action RevertSpeed;

    private void Awake()
    {
        _currentSpeed = _startSpeed;
        _currentReducingCoefficient = _digReducingCoefficient;
    }

    public void IncreaseSpeed(List<AntMionMover> movers, int addedLevel)
    {
        _currentSpeed += (_speedStep * addedLevel);
        SetSpeedAllAnt(movers);
    }

    public void ActivateSpeedBoost(int increasingPrecent, float increasingTime)
    {
        _speedBoostCoefficient = (float)increasingPrecent / 100;
        StartCoroutine(WaitDeactivateSpeedBoost(increasingTime));
    }

    public void SetSpeedAllAnt(List<AntMionMover> movers)
    {
        foreach (var mover in movers)
        {
            SetSpeed(mover);
        }
    }

    public void SetSpeed(AntMionMover mionMover)
    {
        mionMover.SetDurationMove(_totalSpeed);
    }

    public void IncriaseDiggingSpeed(List<AntMionMover> movers, int level)
    {
        for (int i = 0; i < level; i++)
        {
            _currentReducingCoefficient += (CalculateDigReducingStep());
        }
        foreach (var mover in movers)
        {
            SetDiggingDelay(mover);
        }
    }

    private float CalculateDigReducingStep()
    {
        int maxLevel = 3;
        int increasedDigCoefficient = 2;
        float maxDigReducingStep = _digReducingStep * increasedDigCoefficient;
        float level = (_currentReducingCoefficient - _digReducingCoefficient) / maxDigReducingStep;
        if (level < maxLevel)
        {
            return maxDigReducingStep;
        }
        return _digReducingStep;
    }

    public void SetDiggingDelay(AntMionMover mionMover)
    {
        float delay = _startDigDelay / _currentReducingCoefficient;
        mionMover.SetDelay(delay);
    }

    private IEnumerator WaitDeactivateSpeedBoost(float delay)
    {
        yield return new WaitForSeconds(delay);
        _speedBoostCoefficient = 1f;
        RevertSpeed?.Invoke();
    }
}
