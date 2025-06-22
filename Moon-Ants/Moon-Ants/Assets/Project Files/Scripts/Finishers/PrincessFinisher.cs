using System.Collections;
using UnityEngine;

public class PrincessFinisher : Finisher
{
    [SerializeField] private Exploder _explodedCage;
    [SerializeField] private Transform[] _finishTargets;
    [SerializeField] private float _returnDelay;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _effectDelay;

    protected override IEnumerator WaitLaunch()
    {
        _baza.StopAllAnts();
        yield return new WaitForSeconds(_effectDelay);
        ReturnPlayerToPath(_finishTargets);
        yield return new WaitForSeconds(_returnDelay);
        _explodedCage.Activate(_explodedCage.transform.position);     
        yield return new WaitForSeconds(_attackDelay);
        Launch();
        yield return new WaitForSeconds(ShowFinishCanvasDelay);
        ShowFinishCanvas();
    }

    protected override void Launch()
    {
    }   
}
