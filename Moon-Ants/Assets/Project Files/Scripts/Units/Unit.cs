using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IActivable
{
    [SerializeField] private int _health;
    [SerializeField] private ParticleSystem _diedEffect;

    public bool IsActive { get; private set; } = true;

    public event Action<AttackData> Attacked; 
    public event Action<Unit> Died;

    private void OnValidate()
    {
        if (_health <= 0)
            _health = 1;
    }

    public abstract void TryTakeDamage(AttackData attackData);

    protected void TakeDamage(AttackData attackData)
    {
        if (_health <= 0)
            return;

        _health -= attackData.Damage;
        Attacked?.Invoke(attackData);
        if (_health <= 0)
        {
            IsActive = false;
            Died?.Invoke(this);
            TryPlayDiedEffect();
            Kill();
        }
    }

    protected abstract void Kill();

    public void TryPlayDiedEffect()
    {
        if (_diedEffect != null)
            Instantiate(_diedEffect, transform.position, Quaternion.identity);
    }
}

public struct AttackData
{
    public readonly int Damage;
    public readonly Vector3 AttackPoint;

    public AttackData(int damage, Vector3 attackPoint)
    {
        Damage = damage;
        AttackPoint = attackPoint;
    }
}
