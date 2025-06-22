using System.Collections;
using UnityEngine;

public class AttackDistantionTransition : Transition
{
    [SerializeField] private float _attackDistantion = 0.5f;
    [SerializeField] private AudioSource _audioSource;

    private EnemyTargetSelector _targetSelector;
    private Coroutine _waitCheckDistance;

    private bool _isChecking = false;
    private void Awake()
    {
        _targetSelector = GetComponent<EnemyTargetSelector>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        TryStopChecking();
        if (_targetSelector.TargetType != TargetType.Ant)
            return;
        _isChecking = true;
        _waitCheckDistance = StartCoroutine(WaitCheckDistance());
    }

    private void OnDisable()
    {
        TryStopChecking();
    }

    private IEnumerator WaitCheckDistance()
    {
        while (_isChecking)
        {
            if(_targetSelector.Target == null)
            {
                _isChecking = false;
            }
            if (_targetSelector.TargetType != TargetType.Ant)
            {
                _isChecking = false;
            }
            if (Vector3.Distance(transform.position, _targetSelector.Target.position) < _attackDistantion)
            {
                _audioSource.Play();
                NeedTransite = true;
                _isChecking = false;
            }
            yield return new WaitForSeconds(0.2f);
        }

    }
    private void TryStopChecking()
    {
        _isChecking = false;
        if (_waitCheckDistance != null)
            StopCoroutine(_waitCheckDistance);
    }

    private void Activate()
    {
        NeedTransite = true;
    }
}
