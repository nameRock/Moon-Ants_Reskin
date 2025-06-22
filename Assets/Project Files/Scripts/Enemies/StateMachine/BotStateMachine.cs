using System;
using UnityEngine;

public class BotStateMachine : MonoBehaviour
{
    [SerializeField] private State _firstState;
 
    private State _currentState;
    private Enemy _enemy;

    private void Awake()
    {
        if (_firstState == null)
            throw new NullReferenceException();
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        _currentState = _firstState;
        _currentState.EnterState();
    }

    private void OnEnable()
    {
        _enemy.Died += OnDied;
    }

    private void OnDisable()
    {
        _enemy.Died -= OnDied;
    }

    private void Update()
    {
        State nextState = _currentState.TryGetNextState();
        if (nextState != null)
        {
            Transit(nextState);
        }
    }

    public void Deactivate()
    {
        _currentState.Exit();
        enabled = false;
    }

    private void Transit(State nextState)
    {
        _currentState.Exit();
        _currentState = nextState;
        _currentState.EnterState();
    }

    private void OnDied(Unit enemy)
    {
        _currentState.Exit();
        _currentState = null;
        enabled = false;
    }       
}
