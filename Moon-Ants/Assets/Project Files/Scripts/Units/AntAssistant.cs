using System;
using UnityEngine;

public class AntAssistant : Unit
{
    private AntMionMover _mionMover;
    public AntMionMover AntMionMover => _mionMover;

    private void Awake()
    {
        _mionMover = GetComponent<AntMionMover>();
    }

    protected override void Kill()
    {
        _mionMover.Kill();
    }

    public override void TryTakeDamage(AttackData attackData)
    {
        TakeDamage(attackData);
    }
}
