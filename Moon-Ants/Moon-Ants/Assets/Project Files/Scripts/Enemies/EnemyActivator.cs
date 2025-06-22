using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    [SerializeField] private LeaderEnemy[] _enemies;
    [SerializeField] private float _delayRespawn;
    [SerializeField] private GameObject _text;
    [SerializeField] TargetBuilder _targetBuilder;
    [SerializeField] private int _currentIndex = 0;

    private float _currentTime;
    private List<LeaderEnemy> _activatedEnemies = new List<LeaderEnemy>();

    public static event Action<List<LeaderEnemy>> EnemiesCountChanged;

    private void Awake()
    {
        foreach (var enemy in _enemies)
        {
            enemy.Init();
            enemy.Died += OnEnemyDied;
        }
    }

    private void OnEnable()
    {
        _targetBuilder.Builded += StopSpawn;
    }

    private void Start()
    {
        SetCurrentIndex();
        _text.SetActive(_activatedEnemies.Any());
        _activatedEnemies = GetActivatedEnemies();
        
        EnemiesCountChanged?.Invoke(_activatedEnemies);
    }

    private void OnDisable()
    {
        _targetBuilder.Builded -= StopSpawn;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime >= _delayRespawn)
        {
            _currentTime = 0;
            Respawn();
        }
    }

    public void Respawn()
    {
        if (_currentIndex < _enemies.Length)
        {
            _enemies[_currentIndex].gameObject.SetActive(true);
            _activatedEnemies.Add(_enemies[_currentIndex]);
            _currentIndex++;

            _text.SetActive(true);
            EnemiesCountChanged?.Invoke(_activatedEnemies);
        }
    }

    private void OnEnemyDied(Unit unit)
    {
        unit.Died -= OnEnemyDied;
        _activatedEnemies.Remove((LeaderEnemy)unit);
        EnemiesCountChanged?.Invoke(_activatedEnemies);
        _text.SetActive(_activatedEnemies.Any());
    }

    private void StopSpawn()
    {
        enabled = false;
        foreach (var enemy in _enemies)
        {
            if (enemy.gameObject != null && enemy.gameObject.activeSelf == true)
            {
                enemy.StopBehaviour();
            }
        }
    }

    private List<LeaderEnemy> GetActivatedEnemies()
    {
        return _enemies.Where(enemy => enemy.gameObject.activeSelf == true && enemy.IsActive == true).ToList();
    }

    private void SetCurrentIndex()
    {
        for (int i = 0; i < _enemies.Length; i++)
        {
            if(_enemies[i].gameObject == false)
            {
                _currentIndex = i;
                return;
            }
        }
    }
}
