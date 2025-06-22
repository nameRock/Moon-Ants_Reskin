using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/DamageAbility")]
public class DamageAbility : AbilityData
{
    [SerializeField] private int _increasedDamageStep = 2;

    public override void Activate(Player player)
    {
        if (player.PlayerAttacker == null)
        {
            return;
        }
        player.PlayerAttacker.SetIncreasedDamageStep(_increasedDamageStep);
    }

    public override void Deactivate(Player player)
    {
        if (player.PlayerAttacker == null)
        {           
            return;
        }
        player.PlayerAttacker.SetIncreasedDamageStep(0);
    }
}
