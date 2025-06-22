using UnityEngine;

[RequireComponent(typeof(LeaderEnemy))]
public class StunTransition : Transition
{
    [SerializeField] private StunState _stunState;

    private LeaderEnemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<LeaderEnemy>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _enemy.Attacked += OnEnemyAttacked;
    }

    private void OnDisable()
    {
        _enemy.Attacked -= OnEnemyAttacked;
    }

    private void OnEnemyAttacked(AttackData attackData)
    {
        _stunState.SetAttackPoint(attackData.AttackPoint);
        NeedTransite = true;
    }
}
