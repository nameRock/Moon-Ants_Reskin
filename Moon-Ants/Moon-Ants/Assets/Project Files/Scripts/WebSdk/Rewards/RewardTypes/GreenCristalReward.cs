using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCristalReward : Reward
{
    public override void Send(Player player, string course)
    {
        player.Wallet.AddGreenCristals(RewardValue, "Reward");
    }
}
