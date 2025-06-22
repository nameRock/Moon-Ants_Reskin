using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class MoveState : State
{
    [SerializeField] private float _speed = 3;
    [SerializeField, Range(0, 3)] private float _animationSpeed = 0.5f;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;

    private Vector3 _target;

    public event UnityAction TargetReached;

    private void Update()
    {
        Move();
    }

    private void OnDisable()
    {
        StopMove();
    }

    protected abstract Vector3 GetNextTarget();
    protected abstract void Move();

    protected void SetTarget()
    {
        _target = GetNextTarget();
    }

    protected void SetSpeed()
    {
        _agent.speed = _speed;
    }

    protected void StopMove()
    {
        SetAnimationSpeed(0);
        if (_agent.isActiveAndEnabled == true)
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
        }
    }

    protected void ActivateMove()
    {
        _agent.isStopped = false;
    }

    protected bool CheckMinDistance(float minDistance)
    {
        if (Vector3.Distance(transform.position, _target) < minDistance)
        {
            TargetReached?.Invoke();
            return true;
        }
        return false;
    }

    protected bool IsPatchCalculated(Vector3 target)
    {
        return _agent.CalculatePath(target, new NavMeshPath());
    }

    protected void SetVelocityAsMaxSpeed()
    {
        Vector3 direction = (_target - transform.position).normalized;
        Vector3 velocity = _agent.speed * direction;
        _agent.velocity = velocity;
    }

    protected void StartMoveAnimation()
    {
        SetAnimationSpeed(_animationSpeed);
    }

    private void SetAnimationSpeed(float speed)
    {
        _animator.SetFloat(AnimatorConst.Speed, speed);
    }    

    protected void MoveToTarget()
    {
        _agent.SetDestination(_target);
    }
}
