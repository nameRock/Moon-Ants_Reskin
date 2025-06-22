using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FadeCanvasAnimation))]

public class OxygenBar : MonoBehaviour
{
    [SerializeField] private float _fillSpeed;
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private Gradient _gradient;

    private OxygenDetector _oxygen;
    private FadeCanvasAnimation _fadeAnimation;
    private Camera _camera;
    private Coroutine _oxigenChanged;
    private bool _isActive = false;

    private void Awake()
    {
        _camera = Camera.main;
        _oxygen = GetComponentInParent<OxygenDetector>();
        _fadeAnimation = GetComponent<FadeCanvasAnimation>();
    }

    private void OnEnable()
    {
        _oxygen.Changed += OnChanged;
        _oxygen.FullOxygenRecove += OnFullOxygenRecove; 
    }

    private void OnDisable()
    {
        _oxygen.Changed -= OnChanged;
        _oxygen.FullOxygenRecove -= OnFullOxygenRecove;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.back, _camera.transform.rotation * Vector3.up);
    }

    private void OnChanged(float valueAsPercentage)
    {
        if (_isActive == false && _oxygen.OxygenFulled == false)
        {
            _isActive = true;
            _fadeAnimation.ChangeAlpha(1);           
        }
        
        if (_oxigenChanged != null)
        {
            StopCoroutine(_oxigenChanged);
        }
        _healthBarImage.color = _gradient.Evaluate(valueAsPercentage);
        _oxigenChanged = StartCoroutine(HealthChanged(valueAsPercentage));
    }

    private void OnFullOxygenRecove()
    {
        Deactivate();
    }    

    private void Deactivate()
    {
        _isActive = false;
        _fadeAnimation.ChangeAlpha(0);
    }

    private IEnumerator HealthChanged(float valueAsPercentage)
    {
        while (_healthBarImage.fillAmount != valueAsPercentage)
        {
            _healthBarImage.color = _gradient.Evaluate(valueAsPercentage);
            _healthBarImage.fillAmount = Mathf.MoveTowards(_healthBarImage.fillAmount, valueAsPercentage, _fillSpeed * Time.deltaTime);
            yield return null;
        }
    }
}