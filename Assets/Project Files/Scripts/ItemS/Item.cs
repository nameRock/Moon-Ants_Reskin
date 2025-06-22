using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private List<Element> _elements;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ItemSpawner _itemSpawner;
    /*[SerializeField] private Baza _baza;
    [SerializeField] private EnemiesBaza _enemiesBaza;*/
    [SerializeField] private GameObject _circle;
    [SerializeField] private Element _templateElement;
    [SerializeField] private ItemsSaver _itemsSaver;
    [SerializeField] private bool _collectedItem = true;
    [SerializeField] private GameObject _arrowHelper;
    [SerializeField] private GameObject _arrowHelper2;
    [SerializeField] private GameObject _arrowHelper3;

    private int _currentElement = -1;
    private int _currentActiveElement = -1;
    private int _colletedElement = 0;
    private float _distance;
    private bool _isCompleted = false;

    public bool IsEnergy => _elements[0].IsEnerge;
    public bool IsCompleted => _isCompleted;
    public Vector3 Offset => _offset;
    public float Distance => _distance;
    
    public  event Action<Item> Completed;


    private void Start()
    {      
        if (Data.IsSeted == true)
        {
            Debug.Log(gameObject + "  Start()");
            SetSavedData();
            return;
        }
        Data.Setted += OnDataSeted;
    }

    private void OnDataSeted()
    {
        Debug.Log(gameObject + "  OnDataSeted");
        Data.Setted -= OnDataSeted;
        SetSavedData();
    }

    private void SetSavedData()
    {
        if (_collectedItem)
        {
            Debug.Log(gameObject + "  SetSavedData()");
            _itemsSaver.Load();
            StartActivateItem();
        }
    }

    public Element GetNextElement()
    {
        _currentElement++;

        if (_currentElement < _elements.Count)
        {
            _elements[_currentElement].DisableCollider();
            return _elements[_currentElement];
        }
        else
        {
            //_itemSpawner.SpawnElement(_spawnPoint, _offset);
            if (_collectedItem)
            {
                _isCompleted = true;
                Completed?.Invoke(this);
                /*_baza.RemoveItem(this);

                if(_enemiesBaza != null)
                {
                    _enemiesBaza.RemoveItem(this);
                }*/
            }
            _currentElement = _elements.Count - 1;
            return null;
        }
    }

    public void ActivateHelper()
    {
        if (_arrowHelper != null)
        {
            if (_arrowHelper.activeSelf)
            {
                _arrowHelper.SetActive(false);
                if (_arrowHelper3 != null)
                {
                    _arrowHelper3.SetActive(true);
                }
                if (_arrowHelper2 != null)
                {
                    _arrowHelper2.SetActive(true);
                }
            }
        }
    }

    private void StartActivateItem()
    {
        Debug.Log(gameObject + "  StartActivateItem()");
        if (_collectedItem)
        {
            _currentElement = _itemsSaver.GetIndexItem();
            _currentActiveElement = _itemsSaver.GetIndexItem();
            Debug.Log(gameObject.name + "  " + _itemsSaver.Name + "  " + _itemsSaver.GetIndexItem());
            _colletedElement = _currentElement;

            if (_currentElement >= 0)
            {
                if (_currentElement < _elements.Count)
                {
                    for (int i = 0; i <= _currentElement; i++)
                    {
                        _elements[i].DisableCollider();
                        _elements[i].gameObject.SetActive(false);
                    }
                }
                else
                {            
                    _isCompleted = true;
                    Completed?.Invoke(this);
                    gameObject.SetActive(false);
                }
                _itemsSaver.Save();
            }
        }
    }

    public void DisableElement()
    {
        _currentActiveElement++;

        if (_collectedItem)
        {
            _itemsSaver.SetIndexItem(_currentActiveElement);
            _itemsSaver.Save();
        }
    }

    public void GetSpawnPoint(Transform point, ItemSpawner itemSpawner, Vector3 offset)
    {
        _spawnPoint = point;
        _itemSpawner = itemSpawner;
        _offset = offset;
    }

    public void CheckLastElements(Element element)
    {
        _colletedElement++;
        int lastIndex = _elements.Count - 1;
        if (_colletedElement >= lastIndex)
        {
            DeactivateHelpers();
        }
        if(element == _elements[lastIndex])
        {
            DeactivateHelpers();
        }
    }

    public Element GiveAwayTemplate()
    {
        return _templateElement;
    }

    public void SetDistance(Vector3 startPosition)
    {
        _distance = Vector3.Distance(startPosition, transform.position);
    }

    public void AddElement(Element element)
    {
        _elements.Add(element);
    }

    private void DeactivateHelpers()
    {
        if (_circle != null)
        {
            _circle.SetActive(false);
        }
        if (_arrowHelper != null)
        {
            _arrowHelper.SetActive(false);
        }
    }
}
