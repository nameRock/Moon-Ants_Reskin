using UnityEngine;

public class AntsSpeedReward : Reward
{
    [SerializeField] private float _duration = 60;

    public override void Send(Player player, string source)
    {
        Baza baza = player.PlayerCollector.Baza;
        baza.ActivateSpeedBoost(RewardValue, _duration);
    }
}
