using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenDetector : MonoBehaviour
{
    [SerializeField] private int _maxOxigenReserve = 50;
    [SerializeField] private int _lossingReserve = 1;
    [SerializeField] private float _lossingDelay = 0.1f;
    [SerializeField] private int _recovinReserve = 1;
    [SerializeField] private float _recovingDelay = 0.1f;
    
    private List<OxygenZone> _oxygensZones = new List<OxygenZone>();
    private Coroutine _waitRecoveOxygen;
    private Coroutine _waitLosseOxygen;

    public bool OxygenFulled => CurrentOxigenReserve == _maxOxigenReserve;

    public event Action<float> Changed;
    public event Action FullOxygenLossed;
    public event Action FullOxygenRecove;

    public int CurrentOxigenReserve { get; private set; }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        CurrentOxigenReserve = _maxOxigenReserve;
        Changed?.Invoke((float)CurrentOxigenReserve / _maxOxigenReserve);
        FullOxygenRecove?.Invoke();
    }

    public void DetectZone(OxygenZone oxygenZone)
    {
        if (_oxygensZones.Contains(oxygenZone))
            return;
        _oxygensZones.Add(oxygenZone);
        if (_oxygensZones.Count == 1)
        {
            if (_waitLosseOxygen != null)
                StopCoroutine(_waitLosseOxygen);
            TryRecoveOxygen();
        }
    }

    public void LoseZone(OxygenZone oxygenZone)
    {
        if (_oxygensZones.Contains(oxygenZone) == false)
            throw new System.Exception("Not set oxygen zone");
        _oxygensZones.Remove(oxygenZone);
        if (_oxygensZones.Count == 0)
        {
            if (_waitRecoveOxygen != null)
                StopCoroutine(_waitRecoveOxygen);
            TryLoseOxygen();
        }
    }    

    private void TryLoseOxygen()
    {
        if (CurrentOxigenReserve <= 0)
            return;
        if (_waitLosseOxygen != null)     
            StopCoroutine(_waitLosseOxygen);  
        _waitLosseOxygen = StartCoroutine(WaitLosseOxygen());
    }

    private void TryRecoveOxygen()
    {
        if (CurrentOxigenReserve >= _maxOxigenReserve)
            return;
        if (_waitRecoveOxygen != null)
            StopCoroutine(_waitRecoveOxygen); 
        _waitRecoveOxygen = StartCoroutine(WaitRecoveOxygen());
    }

    private IEnumerator WaitLosseOxygen()
    {
        while (CurrentOxigenReserve > 0)
        {
            ChangeOxigenReserve(-_lossingReserve);           
            yield return new WaitForSeconds(_lossingDelay);
        } 
        FullOxygenLossed?.Invoke();
    }

    private IEnumerator WaitRecoveOxygen()
    {
        while (CurrentOxigenReserve < _maxOxigenReserve)
        {
            ChangeOxigenReserve(_recovinReserve);
            yield return new WaitForSeconds(_recovingDelay);
        }
        FullOxygenRecove?.Invoke();
    }

    private void ChangeOxigenReserve(int deltaValue)
    {
        CurrentOxigenReserve += deltaValue;
        CurrentOxigenReserve = Mathf.Clamp(CurrentOxigenReserve, 0, _maxOxigenReserve);
        Changed?.Invoke((float)CurrentOxigenReserve / _maxOxigenReserve);
    }
}