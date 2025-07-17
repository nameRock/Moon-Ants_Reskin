using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCristalReward : Reward
{
    [SerializeField] private string _postfix;

    public override void Send(Player player, string source)
    {
        player.Wallet.AddRedCristals(RewardValue, source);
    }

    public override void GenerateID()
    {
        _rewardId = GetType().FullName + _postfix + IDSeparator + System.Guid.NewGuid().ToString();
    }
}
