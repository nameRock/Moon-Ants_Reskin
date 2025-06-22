using UnityEngine;

public class StateAfterRouterTransition : Transition
{
    [SerializeField] private RouteState _routeState;

    protected override void OnEnable()
    {
        base.OnEnable();
        _routeState.Routed += OnRouted;
        if (_routeState.NextState != null)
        {
            OnRouted(_routeState.NextState);
        }
    }

    private void OnDisable()
    {
        _routeState.Routed -= OnRouted;
    }

    private void OnRouted(State state)
    {
        SetState(state);
        NeedTransite = true;
    }
}
