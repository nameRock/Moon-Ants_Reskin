using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class OxygenZone : MonoBehaviour
{
    [SerializeField] private float _maxRadius = 7;
    [SerializeField] private int _currencyMaxValue = 14;
    [SerializeField] private float _increasedValueCoefficient = 1.5f;
    [SerializeField] private float _increasedSizeSpeed = 2f;
    [SerializeField] private TMP_Text _textPrice;
    [SerializeField] private float _sizeDuration;
    [SerializeField] private GameObject _textObject;
    [SerializeField] private ZonaSaver _zonaSaver;
    [SerializeField] private AudioSource _audioSource;

    private float _startRadius;
    private float _targetRadius;
    private int _currencyCurrentValue = 0;
    private int _currentValue;
    private float _radiusStep;
    private Coroutine _waitIncreaseSize;
    private Tweener _scaledTween;
    private float _currentDelay;
    private float _delay = 0.05f;

    public bool IsFullOpened => _currencyCurrentValue >= _currencyMaxValue;

    public event Action Opened;

    private void Awake()
    {
        _currentDelay = _delay;
        _startRadius = transform.localScale.x;
        _radiusStep = (_maxRadius - _startRadius) / _currencyMaxValue;

        if (_textPrice != null)
        {
            _textPrice.text = "x" + _currencyMaxValue.ToString();
        }
    }

    private void Start()
    {
        if (Data.IsSeted == true)
        {
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
        if (_zonaSaver != null)
        {
            _zonaSaver.Load();

            if (_zonaSaver.GetValueZone())
            {
                _targetRadius = _maxRadius;
                ChangedSize();
                StartCoroutine(CheckComplete());
                _audioSource.Play();
            }

            _zonaSaver.Save();
        }
    }

    private void OnValidate()
    {
        if (_maxRadius < transform.localScale.x)
            _maxRadius = transform.localScale.x;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out OxygenDetector oxygenDetector))
        {
            oxygenDetector.DetectZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out OxygenDetector oxygenDetector))
        {
            oxygenDetector.LoseZone(this);
        }
    }


    public void StartOpening(Player player)
    {
        if (player.PlayerCollector.ContainsEnerge == true)
            player.PlayerCollector.Drop(transform, true, false, true);

        TryStopOpening();
        _waitIncreaseSize = StartCoroutine(WaitIncreaseSize(player));
    }

    public void TryStopOpening()
    {
        if (_currentValue > 0)
        {
            if (_waitIncreaseSize != null)
            {
                _currentDelay = _delay;
                StopCoroutine(_waitIncreaseSize);
            }
            TryChangeTweenValue(transform.localScale, transform.localScale, 0);
        }
    }

    private IEnumerator WaitIncreaseSize(Player player)
    {

        int value = 1;
        while (player.Wallet.BlueCountValue > 0 || player.PlayerCollector.ContainsEnerge == true)
        {
            if (IsFullOpened == true)
                break;

            int maxValue = Math.Min(player.Wallet.BlueCountValue, _currencyMaxValue - _currencyCurrentValue);
            value = Mathf.Clamp(value, 0, maxValue);

            player.Wallet.SpendBlueCristals(value, "Opening", "Zona");
            _currencyCurrentValue += value;
            _targetRadius = _startRadius + _currencyCurrentValue * _radiusStep;
            //delay = (_targetRadius - transform.localScale.x) / _increasedSizeSpeed;
            //IncreaseSize(delay);

            _currentValue = _currencyMaxValue - _currencyCurrentValue;
            _textPrice.text = "x" + _currentValue.ToString();
            yield return new WaitForSeconds(_currentDelay);

            if (_currentDelay > 0.01)
            {
                _currentDelay -= 0.002f;
            }
            else
            {
                _currentDelay = 0.01f;
                if (value < 3)
                    value++;
            }
            //value = (int)Math.Ceiling(value * _increasedValueCoefficient);
        }

        if (_currentValue == 0)
        {
            _zonaSaver.SetValueZone(true);
            _zonaSaver.Save();

            ChangedSize();
            _audioSource.Play();
        }
    }

    public void OpenFree()
    {
        _targetRadius = _maxRadius;

        _zonaSaver.SetValueZone(true);
        _zonaSaver.Save();

        ChangedSize();
        StartCoroutine(CheckComplete());
    }

    private void ChangedSize()
    {
        _textObject.SetActive(false);

        bool isTweenChanged = TryChangeTweenValue(transform.localScale, new Vector3(_targetRadius, _targetRadius, _targetRadius), _sizeDuration);
        if (isTweenChanged == false)
            StartScaledTween(_targetRadius, _sizeDuration);
    }

    private void IncreaseSize(float delay)
    {
        bool isTweenChanged = TryChangeTweenValue(transform.localScale, new Vector3(_targetRadius, _targetRadius, _targetRadius), delay);
        if (isTweenChanged == false)
            StartScaledTween(_targetRadius, delay);
    }

    private bool TryChangeTweenValue(Vector3 startSize, Vector3 targetSize, float duration)
    {
        if (_scaledTween != null && _scaledTween.active == true)
        {
            _scaledTween.ChangeValues(startSize, targetSize, duration);
            return true;
        }
        return false;
    }

    private void StartScaledTween(float radius, float delay)
    {
        _scaledTween = transform.DOScale(new Vector3(radius, radius, radius), delay).SetEase(Ease.Linear).OnComplete(() => TryComplete());
    }

    private IEnumerator CheckComplete()
    {
        yield return new WaitForSeconds(_sizeDuration);
        Opened?.Invoke();
    }

    private void TryComplete()
    {
        if (IsFullOpened == true)
        {
            if (transform.localScale.x < _maxRadius)
            {
                float delay = (_targetRadius - transform.localScale.x) / _increasedSizeSpeed;
                StartScaledTween(_targetRadius, delay);
                return;
            }
            Opened?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _maxRadius);
    }
}
