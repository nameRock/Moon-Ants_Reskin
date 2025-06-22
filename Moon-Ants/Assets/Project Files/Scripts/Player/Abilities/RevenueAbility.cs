using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/RevenueAbility")]
public class RevenueAbility : AbilityData
{
    [SerializeField] private int _priceStep = 1;
    public override void Activate(Player player)
    {
        player.Wallet.SetAbilityPriceStep(_priceStep);
    }

    public override void Deactivate(Player player)
    {
        player.Wallet.SetAbilityPriceStep(0);
    }
}
