using UnityEngine;

public class UpgraderReward : Reward
{
    [SerializeField] private Upgrader _upgrader;
    [SerializeField] private string _postfix;

    public override void Send(Player player, string source)
    {
        _upgrader.IgnorePayment();
        _upgrader.Button.onClick.Invoke();
    }
    public override void GenerateID()
    {
        _rewardId = GetType().FullName + _postfix + IDSeparator + System.Guid.NewGuid().ToString();
    }
}
