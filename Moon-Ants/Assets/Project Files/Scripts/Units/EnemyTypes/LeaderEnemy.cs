using UnityEngine;

[RequireComponent(typeof(EnemyTargetSelector))]
public class LeaderEnemy : Enemy
{
    private BotStateMachine _stateMachine;

    public void Init()
    {
        _stateMachine = GetComponent<BotStateMachine>();
    }

    public void StopBehaviour()
    {
        _stateMachine.Deactivate();
    }

    protected override void Kill()
    {
        gameObject.SetActive(false);
    }
}
