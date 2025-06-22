using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/StackAbility")]
public class StackAbility : AbilityData
{
    [SerializeField] private int _stackStepValue = 2;

    public override void Activate(Player player)
    {
        player.PlayerCollector.SetStackIncreasedStep(_stackStepValue);
    }

    public override void Deactivate(Player player)
    {
        player.PlayerCollector.SetStackIncreasedStep(0);
    }
}
