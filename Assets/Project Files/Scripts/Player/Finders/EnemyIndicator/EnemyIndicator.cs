using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _indicatorSprite;

    private List<LeaderEnemy> _targets = new List<LeaderEnemy>();

    private void Awake()
    {
        SetState(_targets);
    }

    private void Update()
    {
        RotateIndicator();
    }

    public void SetState(List<LeaderEnemy> targets)
    {
        bool isActive = targets.Any();
        if (isActive == true)
            RotateIndicator();
        enabled = isActive;
        _indicatorSprite.gameObject.SetActive(isActive);
        _targets = targets;
    }

    private void RotateIndicator()
    {
        Transform _nearestTarget = GetNearestTarget();
        if (_nearestTarget == null)
            return;
        Vector3 target = new Vector3(_nearestTarget.position.x, transform.position.y, _nearestTarget.position.z);
        Vector3 direction = target - transform.position;
        if (direction == Vector3.zero)
            return;
        Quaternion targetQuternion = Quaternion.LookRotation(direction, transform.up);
        transform.rotation = targetQuternion;
    }

    private Transform GetNearestTarget()
    {
        float minDistance = float.MaxValue;
        Transform nearestTarget = null;
        foreach (var target in _targets)
        {
            Vector3 direction = target.transform.position - transform.position;
            float directionMagnitude = Vector3.SqrMagnitude(direction);
            if (directionMagnitude < minDistance)
            {
                minDistance = directionMagnitude;
                nearestTarget = target.transform;
            }
        }
        return nearestTarget;
    }
}
