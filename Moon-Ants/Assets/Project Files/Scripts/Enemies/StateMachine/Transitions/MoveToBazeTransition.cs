using System.Collections;
using UnityEngine;

public class MoveToBazeTransition : Transition
{
    [SerializeField] private PlayerCollector _playerCollector;
    [SerializeField] private float _delay = 0.5f;

    private EnemyTargetSelector _targetSelector;
    private Coroutine _waitActivate;

    private void Awake()
    {        
        _targetSelector = GetComponent<EnemyTargetSelector>();
    }

    protected override void OnEnable()
    {
        _playerCollector.CollectingEnded += OnCollectingEnded;
        base.OnEnable();
        if (_playerCollector.IsCollect == false)
        {
            Activate();
        }
    }

    private void OnDisable()
    {
        _playerCollector.CollectingEnded -= OnCollectingEnded;
        if (_waitActivate != null)
        {
            StopCoroutine(_waitActivate);
        }
    }

    private void Activate()
    {
        _targetSelector.SetBazaAsTarget( out Transform target);
        NeedTransite = true;

    }
    private void OnCollectingEnded()
    {
        _waitActivate = StartCoroutine(WaitActivate());
    }

    private IEnumerator WaitActivate()
    {
        yield return new WaitForSeconds(_delay);
        Activate();
    }
}
