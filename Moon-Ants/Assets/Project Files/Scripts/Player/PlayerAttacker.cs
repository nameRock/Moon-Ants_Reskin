using System.Collections;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private const string Attack = "Attack";

    [SerializeField] private float _attackRadius = 0.8f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _reloadTime = 1f;
    [SerializeField] private Transform _attackCentre;
    [SerializeField] private EnemyFinder _enemyFinder;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _hitEffectPrefab;
    [SerializeField] private AudioSource _audioSource;

    private int increasedDamageStep = 0;
    private bool _isReady = true;   

    private void OnEnable()
    {
        _enemyFinder.Finded += OnEnemyFinded;
    }

    private void OnDisable()
    {
        _enemyFinder.Finded -= OnEnemyFinded;
    }

    public void SetIncreasedDamageStep(int step )
    {
        increasedDamageStep = step;
    }

    private void OnEnemyFinded(Enemy enemy)
    {
        if (_isReady == true)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        _isReady = false;
        StartCoroutine(WaitReloadAttack());
        _animator.SetTrigger(Attack);
        _audioSource.Play();
    }

    private IEnumerator WaitReloadAttack()
    {
        float timeOffset = 0.1f;
        yield return new WaitForSeconds(_reloadTime + timeOffset);
        _isReady = true;
        TryStartNextAttack();
    }

    private void TryStartNextAttack()
    {
        if(_enemyFinder.IsDetected == true)
        {
            StartAttack();
        }        
    }

    //Call from Animator
    private void TryHitTarget()
    {
        AttackData attackData = new AttackData(_damage + increasedDamageStep, transform.position);
        var hitsInfo = Physics.OverlapSphere(_attackCentre.position, _attackRadius);     
        foreach (var hitInfo in hitsInfo)
        {
            if (hitInfo.TryGetComponent(out Enemy enemy))
            {
                enemy.TryTakeDamage(attackData);
                if (_hitEffectPrefab != null)
                    Instantiate(_hitEffectPrefab, _attackCentre.position, Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackCentre.position, _attackRadius);
    }
}
