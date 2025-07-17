using System;
using System.Collections;
using UnityEngine;

public class RouteState : State
{
    [SerializeField] private PatrolState _patroleState;
    [SerializeField] private PursuitState _pursuitState;    
    [SerializeField] private float _delay = 1f;
    [SerializeField] private ItemFinder _itemFinder;

    private State _nextState;
    private EnemyTargetSelector _targetSelector;
    private Coroutine _waitNextTarget;

    public State NextState => _nextState;

    public event Action<State> Routed;

    private void Awake()
    {
        _targetSelector = GetComponent<EnemyTargetSelector>();
    }

    private void OnEnable()
    {
        _itemFinder.gameObject.SetActive(false);
        Route();
    }

    private void OnDisable()
    {
        _nextState = null;
        if (_waitNextTarget != null)
            StopCoroutine(_waitNextTarget);
    }

    private void Route()
    {
        if (_targetSelector.Target == null || _targetSelector.Target.transform == null)
        {
            if (_waitNextTarget != null)
                StopCoroutine(_waitNextTarget);

            _waitNextTarget = StartCoroutine(WaitNextTarget());
            return;
        }
        if (_targetSelector.TargetType == TargetType.Ant)
        {
            _nextState = _pursuitState;
        }
        else if (_targetSelector.TargetType == TargetType.Item)
        {
            _nextState = _patroleState;
            _itemFinder.gameObject.SetActive(true);
        }
        else if(_targetSelector.TargetType == TargetType.Baza)
        {
            _nextState = _patroleState;
        }
        Routed?.Invoke(_nextState);
    }

    private IEnumerator WaitNextTarget()
    {        
        yield return new WaitForSeconds(_delay);
        _targetSelector.SetNewTarget(out Transform target);
        Route();
    }
}
