using Agava.YandexGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject _canvasUpgrade;
    [SerializeField] private EndCanvas _endCanvas;
    [SerializeField] private GameObject _canvasRevarde;
    [SerializeField] private GameObject _canvasBoard;
    [SerializeField] private GameObject _canvasLocalization;
    [SerializeField] private SkinsDisplay _canvasSkins;
    [SerializeField] private Image _soundButtonImage;
    [SerializeField] private GameObject _storyCanvasDesktop;
    [SerializeField] private GameObject _storyCanvasMobile;
    [SerializeField] private RewardFlowerDisplay _rewardFlowerDisplay;
    [SerializeField] private LeaderboardView _leaderboardView;
    [SerializeField] private CanvasGroup _redMoneyGroup;
    [SerializeField] private bool _isDisableStoryDisplayInEditor = false;
    private void Awake()
    {
#if UNITY_EDITOR
        if (_isDisableStoryDisplayInEditor == true)
            return;
#endif
        if (Application.isMobilePlatform)
        {
            _storyCanvasMobile.SetActive(true);
        }
        else
        {
            _storyCanvasDesktop.SetActive(true);
        }
    }

    public void DisableUIForFinish()
    {
        //_canvasUpgrade.SetActive(false);
        CanvasGroup upgradeGroup = _canvasUpgrade.GetComponent<CanvasGroup>();
        if (_redMoneyGroup.gameObject.activeSelf == false)
            _redMoneyGroup.gameObject.SetActive(true);
        _redMoneyGroup.ignoreParentGroups = true;
        upgradeGroup.alpha = 0.01f;
        _redMoneyGroup.alpha = 1;
        _canvasRevarde.SetActive(false);
        _canvasBoard.SetActive(false);
        _canvasLocalization.SetActive(false);
        _canvasSkins.gameObject.SetActive(false);
        _rewardFlowerDisplay.Hide();
    }

    public void ActivateEndCanvas()
    {
        _endCanvas.gameObject.SetActive(true);
        _endCanvas.ActivationTimer();
    }

    public void HideSoundImage()
    {
        _soundButtonImage.enabled = false;
    }

    public void DisableLiderboardButton()
    {
        _leaderboardView.DisableButton();
    }

    public void EnableLeaderboardButton()
    {
        _leaderboardView.EnableButton();
    }

    public void DisableSkinDisplayButton()
    {
        _canvasSkins.DisableButton();
    }

    public void EnableSkinDisplayButton()
    {
        _canvasSkins.EnableButton();
    }
}