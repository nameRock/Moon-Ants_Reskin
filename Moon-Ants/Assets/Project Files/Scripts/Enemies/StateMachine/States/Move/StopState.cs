using UnityEngine;

public class StopState : MoveState
{
    private void OnEnable()
    {     
        StopMove();
    }

    protected override Vector3 GetNextTarget()
    {
        return transform.position;        
    }

    protected override void Move()
    {
    }
}
