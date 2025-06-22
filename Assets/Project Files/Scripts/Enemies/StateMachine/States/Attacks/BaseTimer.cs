using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BaseTimer : MonoBehaviour
{
    [SerializeField] private float _delay = 3f;

    private Coroutine _waitEndCounted;
    private bool _isCounted = true;

    public bool IsCounted => _isCounted;

    public event UnityAction TimeCounted;

    public void StartCounting()
    {
        if (_isCounted == false && _waitEndCounted != null)
        {
            StopCoroutine(_waitEndCounted);
        }
        _waitEndCounted = StartCoroutine(WaitEndCounting());
    }

    public void SetDelay(float activatingDelay)
    {
        _delay = activatingDelay;
    }

    private IEnumerator WaitEndCounting()
    {
        _isCounted = false;
        yield return new WaitForSeconds(_delay);
        _isCounted = true;
        TimeCounted?.Invoke();
    }

    public void StopCounting()
    {
        if (_waitEndCounted != null)
            StopCoroutine(_waitEndCounted);
        _isCounted = true;
    }
}
