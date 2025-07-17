using UnityEngine;

public abstract class Reward : MonoBehaviour
{
    [SerializeField] private int _rewardCount;
    [SerializeField] private string _beginningSign;
    [SerializeField] private bool _isDescriptionAsPercent = false;
    [SerializeField] private Sprite _rewardIcon;

    protected string _rewardId;

    public const string IDSeparator = "/*";

    public string RewardID => _rewardId;
    public string Description => _beginningSign + GetRewardDescription();
    public int RewardValue => _rewardCount;

    public Sprite RewardIcon => _rewardIcon;

    public abstract void Send(Player player, string source);

    public virtual void GenerateID()
    {
        _rewardId = GetType().FullName + IDSeparator + System.Guid.NewGuid().ToString();
    }

    public void Multiply(int multiplier)
    {
        _rewardCount *= multiplier;
    }

    private string GetRewardDescription()
    {
        if (_isDescriptionAsPercent == false)
            return _rewardCount.ToString();
        return ((float)_rewardCount / 100).ToString();
    }
}