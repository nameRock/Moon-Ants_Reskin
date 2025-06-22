using UnityEngine;

public class CollectingTransition : Transition
{
    [SerializeField] private PlayerCollector _playerCollector;

    protected override void OnEnable()
    {
        _playerCollector.CollectingStarted += OnCollictingStarted;
        base.OnEnable();     
        if (_playerCollector.IsCollect)
            NeedTransite = true;
    }

    private void OnDisable()
    {
        _playerCollector.CollectingStarted -= OnCollictingStarted;
    }

    private void OnCollictingStarted()
    {
        NeedTransite = true;
    }
}
