using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFinisher : Finisher
{
    [SerializeField] private GameObject _deactivationRobot;
    [SerializeField] private Animator _robotAnimator;
    [SerializeField] private CharacterMover _robotMover;
    [SerializeField] private RocketMover _rocketMover;
    [SerializeField] private Exploder[] _explodedWals; 
    [SerializeField] private float _returnDelay;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _effectDelay;
    [SerializeField] private AudioSource _audioSource;

    private float _dilayAttack = 1.3f;

    protected override IEnumerator WaitLaunch()
    {
        _baza.StopAllAnts();
        yield return new WaitForSeconds(_effectDelay);
        _deactivationRobot.SetActive(false);
        _robotAnimator.gameObject.SetActive(true);
        _robotAnimator.enabled = true;
        _robotMover.MoveToPatch();
        yield return new WaitForSeconds(_attackDelay);
        _robotAnimator.SetTrigger(AnimatorConst.Attack);
        yield return new WaitForSeconds(_dilayAttack);
        _audioSource.Play();
        yield return new WaitForSeconds(0f);
        foreach (var wals in _explodedWals)
        {
            wals.Activate(_robotMover.transform.position);                
        }
        yield return new WaitForSeconds(_returnDelay);

        ReturnAll();
        yield return new WaitForSeconds(FlyDelay);
        Launch();
        yield return new WaitForSeconds(ShowFinishCanvasDelay);
        ShowFinishCanvas();
    }
    protected override void Launch()
    {
        _rocketMover.Move();
    }
}
