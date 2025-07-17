using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Baza : MonoBehaviour
{
    [SerializeField] private Item[] _item;
    [SerializeField] protected AntMionMover _antPrefab;
    [SerializeField] private AntMionMover _antPrefabBonus;
    [SerializeField] private Transform _spawnPoint;
    [Header("ItemsAccounting")]
    [SerializeField] private ItemAccounting _itemAccounting = ItemAccounting.NotDistance;
    [SerializeField] private int _maxItemsCount = 3;
    [SerializeField] private Data _data;
    [SerializeField] private GameObject _arrowHelper;
    [SerializeField] protected TargetBuilder _targetBuilder;
    [SerializeField] private ParticleSystem _poof;

    private int _currentIndexItem = -1;
    private List<Item> _currentItems;
    private List<AntMionMover> _antMionMovers;
    private List<AntMionMover> _antMionMoversBonus;
    private AntParametersSetter _antParametersSetter;

    protected Data Data => _data;

    public AntParametersSetter AntParametersSetter => _antParametersSetter;
    public IReadOnlyList<AntMionMover> AntMionMovers => _antMionMovers;

    public event Action AntDied;

    private void Awake()
    {
        _antParametersSetter = GetComponent<AntParametersSetter>();
        _currentItems = new List<Item>();
        _antMionMovers = new List<AntMionMover>();
        _antMionMoversBonus = new List<AntMionMover>();
        if (_itemAccounting == ItemAccounting.Distance)
        {
            foreach (var item in _item)
            {
                item.SetDistance(transform.position);
            }
        }

        foreach (var item in _item)
        {
            _currentItems.Add(item);
            item.Completed +=OnItemCompleted;
        }
    }

    private void OnEnable()
    {
        _antParametersSetter.RevertSpeed += OnRevertSpeed;
    }

    private void OnDisable()
    {        
        foreach (var ant in _antMionMovers)
        {
            ant.Died -= OnAntDied;
        }

        foreach (var item in _item)
        {
            item.Completed -= OnItemCompleted;
        }


        _antParametersSetter.RevertSpeed -= OnRevertSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerCollector collector))
        {
            if (collector.Baza != this)
                return;
            StartDrop(collector);

            if (_arrowHelper != null)
            {
                if (_arrowHelper.activeSelf)
                {
                    _arrowHelper.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerCollector collector))
        {
            if (collector.Baza != this)
                return;
            EndDrop(collector);
        }
    }

    public abstract void AddValueWallet(Element element, String Character);

    protected abstract void StartDrop(PlayerCollector player);

    protected abstract void EndDrop(PlayerCollector player);

    public Item GetCurrentItem()
    {
        _currentIndexItem++;

        int maxItemCount = GetMaxItemsCount();

        if (_currentIndexItem >= maxItemCount)
        {
            _currentIndexItem = 0;
        }

        if (_currentIndexItem < maxItemCount)
        {
            if (_itemAccounting == ItemAccounting.NotDistance)
                return _currentItems[_currentIndexItem];
            else
            {
                var targetsItems = _currentItems.OrderBy(item => item.Distance).Take(maxItemCount).ToList();
                return targetsItems[_currentIndexItem];
            }
        }
        else
        {
            return null;
        }
    }

    public void ButtonSpawnAnts()
    {
        if (_poof != null)
        {
            _poof.Play();
        }

        SpawnAnt(_antPrefab);
    }

    public AntMionMover SpawnAnt(AntMionMover antMion)
    {
        AntMionMover ant = Instantiate(antMion, _spawnPoint.transform.position, Quaternion.identity);
        ant.SetTarget(_targetBuilder);
        _antMionMovers.Add(ant);
        _antParametersSetter.SetSpeed(ant);
        _antParametersSetter.SetDiggingDelay(ant);
        ant.Died += OnAntDied;
        ant.StartBehavior(this);
        return ant;
    }

    public void LoadSpawnAnts()
    {
        int antsCount = _data.GetCountUpgrade();

        StartCoroutine(StartSpawnAnts(antsCount));
    }

    public void ActivateAntCountBoost(int antCount, float duration)
    {
        StartCoroutine(WaitCompletionCountBoost(antCount, duration));
    }

    private IEnumerator WaitCompletionCountBoost(int antCount, float duration)
    {
        for (int i = 0; i < antCount; i++)
        {
            _antMionMoversBonus.Add(SpawnAnt(_antPrefabBonus));
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void RemoveBonusAnt(AntMionMover antMionMover)
    {
        _antMionMoversBonus.Remove(antMionMover);
        antMionMover.Kill();
    }

    private IEnumerator StartSpawnAnts(int antsCount)
    {
        int levelOffset = 1;
        _antParametersSetter.IncreaseSpeed(_antMionMovers, _data.GetSpeedUpgrade() - levelOffset);
        _antParametersSetter.IncriaseDiggingSpeed(_antMionMovers, _data.GetForceUpgrade() - levelOffset);

        for (int i = 0; i < antsCount; i++)
        {
            yield return new WaitForSeconds(0.5f);

            SpawnAnt(_antPrefab);
        }
    }

    private void OnAntDied(AntMionMover ant)
    {
        ant.Died -= OnAntDied;
        _antMionMovers.Remove(ant);
        AntDied?.Invoke();
    }

    public void ActivateSpeedBoost(int increasingPrecent, float increasingTime)
    {
        _antParametersSetter.ActivateSpeedBoost(increasingPrecent, increasingTime);
        _antParametersSetter.SetSpeedAllAnt(_antMionMovers);
        PlayBuf();
    }

    public void RemoveItem(Item item)
    {
        _currentItems.Remove(item);
    }

    public void AddItem(Item item)
    {
        if (_itemAccounting == ItemAccounting.Distance)
        {
            item.SetDistance(transform.position);
        }
        
        _currentItems.Add(item);
        item.Completed += OnItemCompleted;
    }

    public void IncreaseSpeed()
    {
        _antParametersSetter.IncreaseSpeed(_antMionMovers, 1);
        PlayBuf();
    }

    public void PlayBuf()
    {
        for (int i = 0; i < _antMionMovers.Count; i++)
        {
            _antMionMovers[i].PlayBuf();
        }
        for (int i = 0; i < _antMionMoversBonus.Count; i++)
        {
            if (_antMionMoversBonus[i] != null)
            {
                _antMionMoversBonus[i].PlayBuf();
            }
        }
    }

    private void OnRevertSpeed()
    {
        for (int i = 0; i < _antMionMovers.Count; i++)
        {
            _antParametersSetter.SetSpeed(_antMionMovers[i]);
        }
    }

    public void ReduceDiggingDelay()
    {
        _antParametersSetter.IncriaseDiggingSpeed(_antMionMovers, 1);
        PlayBuf();
    }

    public void ReturnAllAnts(Transform transform)
    {
        for (int i = 0; i < _antMionMovers.Count; i++)
        {
            _antMionMovers[i].MoveToFinish(transform);
        }
    }

    public void StopAllAnts()
    {
        for (int i = 0; i < _antMionMovers.Count; i++)
        {
            _antMionMovers[i].StopMove();
        }
    }

    private int GetMaxItemsCount()
    {
        if (_itemAccounting == ItemAccounting.NotDistance)
        {
            return _currentItems.Count;
        }
        else
        {
            if (_currentItems.Count < _maxItemsCount)
                return _currentItems.Count;
            else
                return _maxItemsCount;
        }
    }

    public void RestartMoveAnts()
    {
        StartCoroutine(StartRestartMoveAnt());
    }

    private IEnumerator StartRestartMoveAnt()
    {
        for (int i = 0; i < _antMionMovers.Count; i++)
        {
            yield return new WaitForSeconds(0.2f);

            if (_antMionMovers[i].CurrentItem == null)
            {
                _antMionMovers[i].GetNextTarget();
            }
        }
    }

    private void OnItemCompleted(Item item)
    {
        RemoveItem(item);
        item.Completed -= OnItemCompleted;
    }

    private enum ItemAccounting
    {
        NotDistance,
        Distance
    }
}
