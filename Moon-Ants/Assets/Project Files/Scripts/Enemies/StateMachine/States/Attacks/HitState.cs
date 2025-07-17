using DG.Tweening;
using UnityEngine;

public class HitState : AttackState
{
    [SerializeField] private float _rotationTime = 0.2f;
    [SerializeField] private AnimatorCallbacker _animatorCallbacker;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _attackCentre;
    [SerializeField] private float _attackRadius = 1f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private ParticleSystem _hitEffectPrefab;

    private AntAssistant _antAssistant;
    private Tween _rotationTween;

    protected override void OnEnable()
    {
        _animatorCallbacker.HitCalled += OnHitCalled;
        base.OnEnable();
    }

    private void OnDisable()
    {
        _animatorCallbacker.HitCalled -= OnHitCalled;
    }

    private void OnHitCalled()
    {
        AttackData attackData = new AttackData(_damage, transform.position);
        if (_antAssistant != null)
            _antAssistant.TryTakeDamage(attackData);
        var hitsInfo = Physics.OverlapSphere(_attackCentre.position, _attackRadius);
        foreach (var hitInfo in hitsInfo)
        {
            if (hitInfo.TryGetComponent(out Player player))
            {
                player.TryTakeDamage(attackData);
                if (_hitEffectPrefab != null)
                    Instantiate(_hitEffectPrefab, _attackCentre.position, Quaternion.identity);
                return;
            }
        }
    }

    protected override void AttackTarget(Transform target)
    {       
        _antAssistant = target.GetComponent<AntAssistant>();
        //_rotationTween = transform.DORotate(target.position, _rotationTime);
        _animator.SetTrigger(AnimatorConst.Attack);
        StartReloadAttack();
    }

    private void OnDestroy()
    {
        if (_rotationTween != null)
            _rotationTween.Kill();
    }
}
