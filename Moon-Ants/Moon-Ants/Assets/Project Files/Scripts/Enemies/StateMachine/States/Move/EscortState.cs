using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortState : MoveState
{
    //[SerializeField] private float _minDistance = 5f;

    //private Bot _currentBot;
    //private Unit _targetUnit;
    //private BotVisibilityRange _visibilityRange;
    //private bool _isStoped;

    //public Unit Target => _targetUnit;

    //private void Awake()
    //{
    //    _currentBot = GetComponent<Bot>();
    //}

    //private void OnEnable()
    //{
    //    _isStoped = false;
    //    _targetUnit = TryGetTarget();
    //    ActivateMove();
    //    SetSpeed();
    //    SetTarget();
    //    MoveToTarget();
    //    StartMoveAnimation();
    //}

    //private Unit TryGetTarget()
    //{
    //    if (_currentBot.Target != null)
    //    {
    //        return _currentBot.Target;
    //    }
    //    return null;
    //}

    //protected override Vector3 GetNextTarget()
    //{
    //    return _targetUnit.transform.position;
    //}

    //protected override void Move()
    //{
    //    SetTarget();
    //    SetSwimpAnimationState();
    //    if (_isStoped == true)
    //    {
    //        float minDistanceOffset = 1;
    //        if (CheckMinDistance(_minDistance + minDistanceOffset))
    //        {
    //            _isStoped = false;
    //            ActivateMove();
    //        }
    //        return;
    //    }
    //    _isStoped = CheckMinDistance(_minDistance);
    //    if (_isStoped == true)
    //    {          
    //        StopMove();
    //        return;
    //    }
    //    MoveToTarget();
    //    StartMoveAnimation();
    //}
    protected override Vector3 GetNextTarget()
    {
        throw new System.NotImplementedException();
    }

    protected override void Move()
    {
        throw new System.NotImplementedException();
    }
}
