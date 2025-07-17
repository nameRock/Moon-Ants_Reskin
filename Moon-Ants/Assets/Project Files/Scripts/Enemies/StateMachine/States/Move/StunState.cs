using UnityEngine;

public class StunState : MoveState
{
    [SerializeField] private float _distance = 2f;

    private Vector3 _attackPoint;
    private Quaternion _startRotation;

    private void OnEnable()
    {
        ActivateMove();
        SetSpeed();
        SetTarget();
        MoveToTarget();
        SetVelocityAsMaxSpeed();
        _startRotation = transform.rotation;
    }

    public void SetAttackPoint(Vector3 attackPoint)
    {
        _attackPoint = attackPoint;
    }

    protected override Vector3 GetNextTarget()
    {
        Vector3 direction = (transform.position - _attackPoint).normalized;
        return transform.position + direction * _distance;
    }

    protected override void Move()
    {       
        transform.rotation = _startRotation;
    }    
}
