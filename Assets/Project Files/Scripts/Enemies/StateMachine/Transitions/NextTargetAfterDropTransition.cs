using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyTargetSelector))]
public class NextTargetAfterDropTransition : Transition
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
        _playerCollector.DropingEnded += OnDropingEnded;
        base.OnEnable();
        if(_targetSelector.TargetType == TargetType.Baza && _playerCollector.ElementsCount == 0)
        {
            StartActivating();
        }
    }

    private void OnDisable()
    {
        _playerCollector.DropingEnded -= OnDropingEnded;
        if(_waitActivate != null)
        {
            StopCoroutine(_waitActivate);
        }
    }

    private void OnDropingEnded()
    {
        StartActivating();
    }

    private void StartActivating()
    {
        if (_waitActivate != null)
            StopCoroutine(_waitActivate);
        _waitActivate = StartCoroutine(WaitActivate());
    }

    private IEnumerator WaitActivate()
    {
        yield return new WaitForSeconds(_delay);
        _targetSelector.SetNewTarget(out Transform transform);
        NeedTransite = true;
    }
}
