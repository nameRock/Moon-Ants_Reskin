using UnityEngine;

public class NextTargetAffterAttackTransition : Transition
{
    [SerializeField] protected AttackState _attackState;

    private EnemyTargetSelector _targetSelector;

    private void Awake()
    {
        _targetSelector = GetComponent<EnemyTargetSelector>();
    }

    protected override void OnEnable()
    {        
        base.OnEnable();
        _attackState.AttackEnded += OnAttackEnded;
        if(_targetSelector.transform == null || _targetSelector.TargetType != TargetType.Ant)
        {
            NeedTransite = true;
        }
    }

    private void OnDisable()
    {
        _attackState.AttackEnded -= OnAttackEnded;
    }

    private void OnAttackEnded()
    {
        NeedTransite = true;
    }
}
