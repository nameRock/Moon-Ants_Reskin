using UnityEngine;
using UnityEngine.UI;

public class RewardButtonStarter : RewardADStarter
{
    [SerializeField] private Button _rewardButton;
    [SerializeField] private SenderEvents _senderEvents;

    protected override void OnEnable()
    {
        base.OnEnable();
        _rewardButton.onClick.AddListener(OnRewardButtonClick);
    }

    protected override void  OnDisable()
    {
        base.OnDisable();
        _rewardButton.onClick.RemoveListener(OnRewardButtonClick);
    }

    private void OnRewardButtonClick()
    {
        if(_senderEvents != null)
        {
            _senderEvents.SendClick();
        }

        ShowRewardAd();
    }
}
