using System.Collections;
using UnityEngine;

public class AlienFinisher : Finisher
{
    [SerializeField] private CharacterMover _alienMover;
    [SerializeField] private RocketMover _rocketMover;
    [SerializeField] private float _returnDelay;
    [SerializeField] private float _alienEffectDelay;

    protected override void Launch()
    {
        _rocketMover.Move();
    }

    protected override IEnumerator WaitLaunch()
    {
        yield return new WaitForSeconds(_alienEffectDelay);
        _alienMover.Move();
        yield return new WaitForSeconds(_returnDelay);
        ReturnAll();
        yield return new WaitForSeconds(FlyDelay);
        Launch();
        yield return new WaitForSeconds(ShowFinishCanvasDelay);
        ShowFinishCanvas();
    }   
}
