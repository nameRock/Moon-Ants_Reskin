using UnityEngine;

public abstract class Transition : MonoBehaviour
{
    [SerializeField] private State _targetState;

    public State TargetState => _targetState;

    public bool NeedTransite { get; protected set; }

    protected virtual void OnEnable()
    {
        NeedTransite = false;
    }
    protected void SetState(State state)
    {
        _targetState = state;
    }
}
