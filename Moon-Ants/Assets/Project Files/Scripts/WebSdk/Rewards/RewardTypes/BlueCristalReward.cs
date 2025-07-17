
public class BlueCristalReward : Reward
{
    public override void Send(Player player, string source)
    {
        player.Wallet.AddBlueCristals(RewardValue, "Reward", "Player");
    }
}
