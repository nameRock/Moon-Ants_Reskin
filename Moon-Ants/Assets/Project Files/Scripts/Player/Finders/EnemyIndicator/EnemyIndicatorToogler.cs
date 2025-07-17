using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyIndicatorToogler : MonoBehaviour
{
    [SerializeField] private EnemyIndicator _enemyIndicator;
    
    private void OnEnable()
    {
        EnemyActivator.EnemiesCountChanged += OnEnemyCounsChanged;
    }

    private void OnDisable()
    {
        EnemyActivator.EnemiesCountChanged -= OnEnemyCounsChanged;
    }

    private void OnEnemyCounsChanged(List<LeaderEnemy> targets)
    {
        _enemyIndicator.SetState(targets);
    }
}
