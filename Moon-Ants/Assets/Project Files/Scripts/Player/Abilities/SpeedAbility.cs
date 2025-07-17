using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SpeedAbility")]
public class SpeedAbility : AbilityData
{
    [SerializeField, Range(1, 2)] private float _speedMultiplier = 1;

    private Player _player;

    public override void Activate(Player player)
    {
        _player = player;
        player.PlayerMover.SetSpeedMultiplier(_speedMultiplier);
    }

    public override void Deactivate(Player player)
    {
        player.PlayerMover.SetSpeedMultiplier(1f);
    }   
}
