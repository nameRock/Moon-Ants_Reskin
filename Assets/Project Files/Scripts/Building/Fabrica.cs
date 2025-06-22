using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class Fabrica : MonoBehaviour
{
    private const string IsJob = "IsJob";

    [SerializeField] private TMP_Text _value;
    [SerializeField] private float _delay;
    [SerializeField] private DeliveryDesk _deliveryDesk;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _template;
    [SerializeField] private GameObject[] _templateElemnts;
    [SerializeField] private Transform _dropTarget;

    private int _maxValue = 12;
    private int _currentValue;
    private bool _isJob = true;
    private float _currentTime;
    private bool _isMove = true;
    private int _movingElementCount = 0;
    private bool _startDrop = false;
    private bool _elementAdded = false;

    public bool IsReady => _currentValue < _maxValue;

    private void Start()
    {
        ChangedTextValue();
    }

    private void Update()
    {
        if (_elementAdded == true & (_currentValue > 0 || _movingElementCount > 0 ))
        {
            if (_deliveryDesk.CheckFreePlace())
            {
                _currentTime += Time.deltaTime;

                Job();
                TryDeactivateElement();

                if (_currentTime >= _delay)
                {
                    _isMove = true;                    
                    _currentTime = 0;
                    _template.SetActive(false);
                    _movingElementCount = 0;
                    _deliveryDesk.SpawnElement();
                    _elementAdded = _currentValue > 0;
                }
            }
            else
            {
                _animator.SetBool(IsJob, false);
            }
        }
        else
        {
            _animator.SetBool(IsJob, false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            if (_isJob && _startDrop == false)
            {
                _startDrop = true;
                StartDrop(player.PlayerCollector);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            _startDrop = false;
            EndDrop(player.PlayerCollector);
        }
    }

    private void StartDrop(PlayerCollector collector)
    {
        collector.Drop(_dropTarget, false, true);
    }

    private void EndDrop(PlayerCollector collector)
    {
        collector.StopDrop();
    }

    public void AddElement(PlayerCollector collector)
    {
        if (_currentValue < _maxValue)
        {
            _currentValue++;
            ChangedTextValue();
        }
        if (_currentValue >= _maxValue)
        {
            collector.StopDrop();
            _isJob = false;
        }
    }

    public void ActivateFreeElement()
    {
        if (_currentValue < _maxValue)
        {
            _isJob = true;
        }
        for (int i = 0; i < _templateElemnts.Length; i++)
        {
            if (_templateElemnts[i].activeSelf == false)
            {
                _templateElemnts[i].SetActive(true);
                break;
            }
        }
        _elementAdded = true;
    }

    private void ChangedTextValue()
    {
        _value.text = _currentValue + "/" + _maxValue;
    }

    private void Job()
    {
        _template.SetActive(true);
        _animator.SetBool(IsJob, true);
    }

    private void TryDeactivateElement()
    {
        if (_isMove)
        {
            _isMove = false;
            _currentValue--;
            ChangedTextValue();
            _isJob = _currentValue < _maxValue;

            for (int i = 0; i < _templateElemnts.Length; i++)
            {
                if (_templateElemnts[i].activeSelf)
                {
                    _templateElemnts[i].SetActive(false);
                    break;
                }
            }
            _movingElementCount = 1;
        }
    }
}
