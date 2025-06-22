using System;
using System.Collections;
using UnityEngine;

public abstract class TargetBuilder : MonoBehaviour
{ 
    [SerializeField] private float _countToSpawn;
    [SerializeField] private int _multiplierElement;
    [SerializeField] private Data _data;
    [SerializeField] private BuildingCounter _buildingCounter;
    [SerializeField] private GameObject _helperArrow;
    [SerializeField] private Transform _antsPoint;
    [SerializeField] private Transform _dropTarget;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private bool _isStopBuilding = false;

    private int _maxElementCount;
    private float _currentValue = -1;
    private bool _isCollected = false;
    private Coroutine _waitStartDrop;

    protected int CurrentIndexElement = 0;
    public event Action Builded;

    public Transform AntsPoint => _antsPoint;

    private void Awake()
    {
        _maxElementCount = GetMaxElementCount();
        if (_dropTarget == null)
            _dropTarget = transform;
    }

    private void Start()
    {
        if (Data.IsSeted == true)
        {
            StartActivateElement();
            return;
        }
        Data.Setted += OnDataSeted;
    }

    private void OnDataSeted()
    {
        StartActivateElement();
        Data.Setted -= OnDataSeted;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.PlayerCollector.Drop(_dropTarget, false);

            if (_helperArrow != null)
            {
                _helperArrow.SetActive(false);
            }
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.PlayerCollector.StopDrop();
        }
    }

    public abstract  void StartActivateElement();

    protected abstract int GetMaxElementCount();

    protected abstract void Builde(string character = "Ant");

    public void AddElement(float value, string character)
    {
        _currentValue += value;
        _buildingCounter.ChangedCountElement();

        CheckAvalibleElements(character);
    }

    public void CheckAvalibleElements(string character)
    {
        if (_currentValue >= _countToSpawn)
        {
            _currentValue -= 1;

            if (CurrentIndexElement < _maxElementCount)
            {
                Builde(character);
            }
            CheckCompletion();
        }
    }

    protected void CheckCompletion()
    {
        if (CurrentIndexElement >= _maxElementCount - 1)
        {
            if (_isCollected == false)
            {
                _isCollected = true;
                if (_playerMover != null && _isStopBuilding)
                {
                    _playerMover.StopRun();
                }

                Builded?.Invoke();
            }
        }
    }

    protected int GetMultiplierElement()
    {
        return _multiplierElement;
    }   

    protected void SetData()
    {
        _data.SetIndexTextRocket();
        _data.SetCurrentIndexRocket(CurrentIndexElement);
    }

    protected void GetData()
    {
        CurrentIndexElement = _data.GetIndexRocket();
        _buildingCounter.GetCountElement(_data.GetIndexTextRocket());
    }
}