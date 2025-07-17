using UnityEngine;

public class AntsCountReward : Reward
{
    [SerializeField] private float _duration = 60;

    public override void Send(Player player, string source)
    {
        player.PlayerCollector.Baza.ActivateAntCountBoost(RewardValue, _duration);
    }
}
