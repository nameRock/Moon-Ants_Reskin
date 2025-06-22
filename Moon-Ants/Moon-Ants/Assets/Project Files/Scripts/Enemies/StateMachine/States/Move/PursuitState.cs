using UnityEngine;

public class PursuitState : MoveState
{
    private EnemyTargetSelector _targetSelector;  

    private void Awake()
    {
        _targetSelector = GetComponent<EnemyTargetSelector>();
    }  

    private void OnEnable()
    {        
        ActivateMove();
        SetSpeed();
        SetTarget();
        MoveToTarget();
        StartMoveAnimation();
    }

    protected override Vector3 GetNextTarget()
    {
        if (_targetSelector.Target == null)
        {
            _targetSelector.SetNewTarget(out Transform target);
            if (target == null)
                return transform.position;
        }
        return new Vector3(_targetSelector.Target.position.x, transform.position.y, _targetSelector.Target.position.z);
    }

    protected override void Move()
    {
        SetTarget();
        MoveToTarget();       
    }

}
