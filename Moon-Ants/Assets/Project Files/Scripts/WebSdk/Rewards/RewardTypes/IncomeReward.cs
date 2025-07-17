using UnityEngine;

public class IncomeReward : Reward
{
    [SerializeField] private float _duration = 60f;
    [SerializeField] private PlayerBaza _playerBaza;

    public override void Send(Player player, string source)
    {
        player.Wallet.ActivatePriceBoost(RewardValue, _duration);
        player.PlayUpgradeEffect();
        _playerBaza.PlayBuf();
    }
}
