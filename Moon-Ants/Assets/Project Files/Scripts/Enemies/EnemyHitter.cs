using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHitter : MonoBehaviour
{
    //[SerializeField] private Animator _animator;
    //[SerializeField] private Transform _attackCentre;
    //[SerializeField] private Weapon _weapon;
    //[SerializeField] private UnitType[] _unitTypes;
    //[SerializeField] private Unit _unit;
    //[SerializeField] private float _swordAttackRadius = 1f;
    //[SerializeField] private int _attackNumber = 1;
    //[SerializeField] private WeaponType _pryorytyWeaponType;
    //[SerializeField] private BotVisibilityRange _visibilityRange;
    //[SerializeField] private List<Weapon> _weapons;
    //[SerializeField] private GameObject _weaponModel;
    //[SerializeField] bool _isIngnoreWeaponType = true;
    //[SerializeField] private Transform _weaponContainer;

    //private float _damage;
    //private WeaponType _curentWeaponType = WeaponType.Axe;
    //private WeaponTypeSwitcher _typeSwitcher;

    //public event UnityAction HitEnded;
    //public event UnityAction TargetHited;

    //private void Awake()
    //{
    //    if (_isIngnoreWeaponType == true)
    //        return;
    //    _typeSwitcher = new WeaponTypeSwitcher(_curentWeaponType, _pryorytyWeaponType);        
    //    ChangeWeapon(_curentWeaponType);
    //}

    //private void OnEnable()
    //{
    //    if (_isIngnoreWeaponType == true)
    //        return;
    //    _visibilityRange.TargetDetected += OnTargetDetected;
    //}

    //private void OnDisable()
    //{
    //    if (_isIngnoreWeaponType == true)
    //        return;
    //    _visibilityRange.TargetDetected -= OnTargetDetected;
    //}

    //private void OnTargetDetected()
    //{
    //    _typeSwitcher.TryGetNewWeaponType(_visibilityRange.Target, out WeaponType weaponType);
    //    _curentWeaponType = weaponType;
    //    ChangeWeapon(_curentWeaponType);
    //}

    //public void SetDamage(float damage)
    //{
    //    _damage = damage;
    //}

    //public void BaseAttack()
    //{
    //    _animator.SetInteger(AnimatorConst.AttackNumber, _attackNumber);
    //    _animator.SetTrigger(AnimatorConst.Attack);
    //}

    ////Ñall from the Animator
    //public void EndHit()
    //{
    //    HitEnded?.Invoke();
    //}

    ////Ñall from the Animator
    //public void HitTarget()
    //{
    //    var hitsInfo = Physics.OverlapSphere(_attackCentre.position, _swordAttackRadius);
    //    foreach (var hitInfo in hitsInfo)
    //    {
    //        if (hitInfo.TryGetComponent(out Unit unit))
    //        {
    //            if (IsTargetUnit(unit) == false)
    //                continue;
             
    //            unit.TryTakeDamage(new AttackData(_weapon, _attackCentre.position, _unit));
    //            TargetHited?.Invoke();
    //        }
    //    }
    //}

    //public void SetTargetUnit(Unit unit)
    //{
    //    _unit = unit;
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
    ////Ñall from the Animator
    //public void ActivateSword()
    //{
    //}

    //public void ChangeWeapon(WeaponType weaponType)
    //{
    //    Destroy(_weaponModel);
    //    _weapon = _weapons.First(weapon => weapon.Type == weaponType);
    //    _weaponModel = Instantiate(_weapon.Model, _weaponContainer);
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    if (_attackCentre != null)
    //        Gizmos.DrawWireSphere(_attackCentre.position, _swordAttackRadius);
    //}
}
