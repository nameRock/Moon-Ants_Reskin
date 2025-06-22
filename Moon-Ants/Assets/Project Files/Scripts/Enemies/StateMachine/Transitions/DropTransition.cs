using UnityEngine;

public class DropTransition : Transition
{
    [SerializeField] private PlayerCollector _playerCollector;

    private EnemyTargetSelector _targetSelector;

    private void Awake()
    {
        _targetSelector = GetComponent<EnemyTargetSelector>();
    }
    protected override void OnEnable()
    {
        _playerCollector.DropingStarted += OnDropingStarted;
        base.OnEnable();
        if (_targetSelector.TargetType == TargetType.Baza && _playerCollector.ElementsCount == 0)
        {
            NeedTransite = true;
        }
    }

    private void OnDisable()
    {
        _playerCollector.DropingStarted -= OnDropingStarted;
    }

    private void OnDropingStarted()
    {        
        NeedTransite = true;
    }
}
