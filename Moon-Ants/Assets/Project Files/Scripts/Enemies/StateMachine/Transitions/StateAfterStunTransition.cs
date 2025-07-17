using System.Collections;
using UnityEngine;

public class StateAfterStunTransition : Transition
{
    [SerializeField] private float _stunDuratin;

    private Coroutine _waitEndingDelay;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_waitEndingDelay != null)
            StopCoroutine(_waitEndingDelay);
        StartCoroutine(WAitEndingDelay(_stunDuratin));
    }

    private IEnumerator WAitEndingDelay( float time)
    {
        yield return new WaitForSeconds(time);
        NeedTransite = true;
    }
}
