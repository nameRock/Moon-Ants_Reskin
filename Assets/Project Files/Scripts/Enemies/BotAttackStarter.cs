using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BotAttackStarter : MonoBehaviour
{
    //[SerializeField] private float _timeReloadAttack = 1f;
    //[SerializeField] private Unit _target;
    //[SerializeField] private UnitType[] _unitTypes;

    //private bool _readyAttackEnemy = true;
    //private float _defaultTimeReloadAttack;

    //public Unit Target => _target;

    //public event UnityAction TargetReached;

    //private void Awake()
    //{
    //    _defaultTimeReloadAttack = _timeReloadAttack;
    //}

    //public void SetTimeReloadAttack(float time)
    //{
    //    _timeReloadAttack = time;
    //}

    //public void SetDefaultTimeReloadAttack()
    //{
    //    _timeReloadAttack = _defaultTimeReloadAttack;
    //}

    //private void OnTriggerStay(Collider collision)
    //{
    //    if (_readyAttackEnemy)
    //    {
    //        if (collision.TryGetComponent<Unit>(out Unit target))
    //        {
    //            if (IsTargetUnit(target) == false)
    //                return;
    //            _target = target;
    //            TargetReached?.Invoke();
    //            StartCoroutine(ReloadAttack());
    //        }
    //    }
    //}

    //private bool IsTargetUnit(Unit unit)
    //{
    //    foreach (var unitType in _unitTypes)
    //    {
    //        if (unitType == unit.Type)
    //            return true;
    //    }
    //    return false;
    //}

    //private void ReachTarget(Unit target)
    //{
    //    _target = target;
    //    TargetReached?.Invoke();
    //    StartCoroutine(ReloadAttack());
    //}

    //private IEnumerator ReloadAttack()
    //{
    //    _readyAttackEnemy = false;
    //    yield return new WaitForSeconds(_timeReloadAttack);
    //    _readyAttackEnemy = true;
    //}
}
