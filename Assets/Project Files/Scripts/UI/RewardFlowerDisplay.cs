using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardFlowerDisplay : MonoBehaviour
{
    [SerializeField] private float _animationDuration = 1f;
    [SerializeField] private Button _showVideoButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Transform _rewardPanel;
    [SerializeField] private TMP_Text _countRewardText;
    [SerializeField] private Image _rewardIcon;
    [SerializeField] private UI _ui;

    private Tween _animation;

    public event Action Clicked;

    private void OnEnable()
    {
        _showVideoButton.onClick.AddListener(() => Clicked?.Invoke());
        _closeButton.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
         _showVideoButton.onClick.RemoveListener(() => Clicked?.Invoke());
        _closeButton.onClick.RemoveListener(Hide);
    }

    public void Show(Reward reward)
    {        
        Init(reward);
        TryStopAnimation();
        gameObject.SetActive(true);
        _rewardPanel.localScale = Vector3.zero;
        _animation = _rewardPanel.DOScale(Vector3.one, _animationDuration).SetEase(Ease.Linear);
        _ui.DisableLiderboardButton();
        _ui.DisableSkinDisplayButton();
    }

    public void Hide()
    {
        TryStopAnimation();
        _animation = _rewardPanel.DOScale(Vector3.zero, _animationDuration).SetEase(Ease.Linear);
        _animation.OnComplete(() => gameObject.SetActive(false));
        _ui.EnableLeaderboardButton();
        _ui.EnableSkinDisplayButton();
    }

    private void Init(Reward reward)
    {
        _countRewardText.text = reward.RewardValue.ToString();
        _rewardIcon.sprite = reward.RewardIcon;
    }

    private void TryStopAnimation()
    {
        if (_animation != null && _animation.active == true)
            _animation.Kill();
    }
}
