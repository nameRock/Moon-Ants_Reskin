using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardEffectCanvas : MonoBehaviour
{
    [SerializeField] private CrystallImage _crystallPrefab;
    [SerializeField] private ParticleSystem _glowEffectPrefab;
    [SerializeField] private RewardView _rewardView;
    [SerializeField] private int _countCrystals = 15;
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _blueCrystalsTransform;
    [SerializeField] private Transform _redCrystalsTransform;
    [SerializeField] private Transform _greenCrystalsTransform;
    [SerializeField] private float _startFlightTime = 2f;
    [SerializeField] private float _secondFlightTime = 0.8f;
    [SerializeField] private Reward _testReward;
    [SerializeField] private Transform _testTransform;
    [SerializeField] private Transform _deffaultStartTransform;

    private Vector3 _startScreenPosition;
    private List<CrystallImage> _crystalls = new List<CrystallImage>();
    private Camera _camera;
    private int _flighMultyplier = 3;
    public Action Completed;
    private void Awake()
    {
        _camera = Camera.main;
    }

    public void Play(Reward reward, bool isWordTarget, Transform startTransform)
    {
        Vector3 effectPosition;

        _startTransform = startTransform;

        if (isWordTarget)
        {
            _startScreenPosition = _camera.WorldToScreenPoint(_startTransform.position);
            effectPosition = Vector3.Lerp(_camera.transform.position, startTransform.position, 0.1f);
        }
        else
        {
            _startScreenPosition = _startTransform.position;
            effectPosition = _startScreenPosition;
        }

        _flighMultyplier = GetFlighMultiplier();
        if (isWordTarget == true)
        {
            ParticleSystem glowEffect = Instantiate(_glowEffectPrefab, effectPosition, Quaternion.identity);
            glowEffect.transform.SetParent(_camera.transform);

        }

        StartCoroutine(WaitPlayCompleting(reward, isWordTarget));
    }

    private int GetFlighMultiplier()
    {
        if (Screen.width > Screen.height)
            return Screen.width / 12;
        else
            return Screen.width / 5;
    }

    private IEnumerator WaitPlayCompleting(Reward reward, bool isWordTarget)
    {
        if (isWordTarget == true)
            yield return new WaitForSeconds(0.3f);
        CreateRewards(reward);
        if (isWordTarget)
        {
            _rewardView.transform.position = _startScreenPosition;
            _rewardView.Show(reward);
            _rewardView.transform.SetSiblingIndex(transform.childCount - 1);
        }
        float delay = _startFlightTime + _secondFlightTime + 0.3f;
        yield return new WaitForSeconds(delay);
        Completed?.Invoke();
    }
    private void CreateRewards(Reward reward)
    {
        Transform secondTargetTransform = GetSecondTargetTransform(reward);

        for (int i = 0; i < _countCrystals; i++)
        {
            CrystallImage crystallImage = Instantiate(_crystallPrefab, transform);
            _crystalls.Add(crystallImage);
            crystallImage.transform.position = _startScreenPosition;
            Vector3 firstTarget = GetRandomTarget();
            Vector3 secondtarget = _camera.WorldToScreenPoint(secondTargetTransform.position);
            crystallImage.SetImage(reward.RewardIcon);
            crystallImage.Move(firstTarget, secondtarget, _startFlightTime, _secondFlightTime);
        }
    }
    private Vector3 GetRandomTarget()
    {
        return _startScreenPosition + Random.insideUnitSphere * _flighMultyplier;
    }

    private Transform GetSecondTargetTransform(Reward reward)
    {
        switch (reward)
        {
            case BlueCristalReward blueCristalReward:
                return _blueCrystalsTransform;
            case RedCristalReward redCristalReward:
                return _redCrystalsTransform;
            case GreenCristalReward greenCristalReward:
                return _greenCrystalsTransform;
        }
        throw new Exception("Not correct reward");
    }
}
