using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyTargetSelector))]
public class PatrolState : MoveState
{
    [SerializeField] private float _patrolRadius = 3f;
    private EnemyTargetSelector _targetSelector;
    private Vector3 _startPosition;
    private float _minDistance = 1f;

    private void Awake()
    {
        _targetSelector = GetComponent<EnemyTargetSelector>();
        _startPosition = transform.position;
    }

    private void OnEnable()
    {
        ActivateMove();
        SetSpeed();
        SetTarget();
        MoveToTarget();
        StartMoveAnimation();
    }

    protected override void Move()
    {  
        if (CheckMinDistance(_minDistance))
        {
            SetTarget();
            MoveToTarget();
        }
    }

    protected override Vector3 GetNextTarget()
    {
        Vector3 position;

        if (_targetSelector.Target != null)
            position = new Vector3(_targetSelector.Target.position.x, transform.position.y, _targetSelector.Target.position.z);
        else
        {
            _targetSelector.SetNewTarget(out Transform target);
            position = new Vector3(_targetSelector.Target.position.x, transform.position.y, _targetSelector.Target.position.z);
            //position = GetRadomTarget();

        }
        return SelectPosition(position);
    }

    private Vector3 SelectPosition(Vector3 position)
    {
        float radius = 0.5f;
        if (GetSamplePosition(position, radius, out Vector3 samplePosition))
        {
            return samplePosition;
        }
        return _startPosition;
    }

    private bool GetSamplePosition(Vector3 sourcePosition, float radius, out Vector3 samplePosition)
    {
        if (NavMesh.SamplePosition(sourcePosition, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            if (IsPatchCalculated(hit.position))
            {
                samplePosition = hit.position;
                return true;
            }
        }
        samplePosition = Vector3.zero;
        return false;
    }

    private Vector3 GetRadomTarget()
    {
        float offSetPosition = 0.1f;
        return new Vector3((Random.insideUnitCircle.x * _patrolRadius) + transform.position.x, transform.position.y + offSetPosition, (Random.insideUnitCircle.y * _patrolRadius) + transform.position.z);
    }
}
