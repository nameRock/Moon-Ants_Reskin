using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyTargetSelector))]
public abstract class AttackState : State
{
    [SerializeField] private float _delayAfterAttack = 0.5f;

    private EnemyTargetSelector _targetSelector;
    private Coroutine _waitReloadAttack;

    public event UnityAction AttackEnded;

    private void Awake()
    {

        _targetSelector = GetComponent<EnemyTargetSelector>();
    } 

    protected virtual void OnEnable()
    {
        AttackTarget(_targetSelector.Target);
    }

    protected abstract void AttackTarget(Transform target);

    protected void StartReloadAttack()
    {
        if (_waitReloadAttack != null)
        {
            StopCoroutine(_waitReloadAttack);
        }
        StartCoroutine(WaitReloadAttack(_delayAfterAttack));
    }

    private IEnumerator WaitReloadAttack(float attackReloadTime)
    {
        yield return new WaitForSeconds(attackReloadTime);
        AttackEnded?.Invoke();
    }
}
