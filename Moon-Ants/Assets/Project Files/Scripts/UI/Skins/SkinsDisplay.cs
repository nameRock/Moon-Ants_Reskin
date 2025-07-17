using System.Linq;
using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class SkinsDisplay : MonoBehaviour
{
    [SerializeField] private SkinView _skinViewPrefabs;
    [SerializeField] private SkinsSelector _skinsSelector;
    [SerializeField] private SimpleScrollSnap _scrollSnap;
    [SerializeField] private SkinsPreviewer _skinsPreviewer;
    [SerializeField] private AbilitiesTextsDisplay _abilitiesInfoDisplay;
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private BayButton _bayButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private GameObject _display;
    [SerializeField] private Image[] _icons;
    [SerializeField] private GameObject _iconTemplate;
    [SerializeField] private LeaderboardView _leaderboardView;

    private List<SkinView> _skinsViews = new List<SkinView>();
    private Player _player;
    private bool _isWaitingCentereView = false;
    private int _lastCenteredPanelIndex = -1;

    private void Awake()
    {
        _player = _skinsSelector.GetComponent<Player>();
    }

    private void OnEnable()
    {
        _skinsSelector.SkinsSetted += OnSkinsSetted;
        _scrollSnap.PanelCentered += OnPanelCentered;
        _scrollSnap.DragEnded += OnDragEneded;
        _scrollSnap.DragBegined += OnDragBegined;
        _openButton.onClick.AddListener(OnOpenButtonClick);
        _closeButton.onClick.AddListener(OnCloseButtonClick);
        _bayButton.Clicked += OnBuyButtonClick;
        _selectButton.onClick.AddListener(OnSelectButtonClick);
    }

    private void OnDisable()
    {
        _skinsSelector.SkinsSetted -= OnSkinsSetted;
        _scrollSnap.PanelCentered -= OnPanelCentered;
        _scrollSnap.DragEnded -= OnDragEneded;
        _scrollSnap.DragBegined -= OnDragBegined;
        _openButton.onClick.RemoveListener(OnOpenButtonClick);
        _closeButton.onClick.RemoveListener(OnCloseButtonClick);
        _bayButton.Clicked -= OnBuyButtonClick;
        _selectButton.onClick.RemoveListener(OnSelectButtonClick);

        ClearViews(_skinsViews);
    }

    private void OnDragBegined()
    {
        _bayButton.gameObject.SetActive(false);
        _selectButton.gameObject.SetActive(false);
    }

    private void OnBuyButtonClick()
    {
        SkinView centeredSkin = _skinsViews[_lastCenteredPanelIndex];
        centeredSkin.OnBayButtonClick();
    }

    private void OnSelectButtonClick()
    {
        SkinView centeredSkin = _skinsViews[_lastCenteredPanelIndex];
        centeredSkin.OnSelectButtonClick();
    }
    
    private List<SkinView> CreateListViews(IReadOnlyCollection<Skin> skins)
    {
        List<SkinView> skinsViews = new List<SkinView>(skins.Count);

        foreach (var skin in skins)
        {
            _scrollSnap.AddToFront(_skinViewPrefabs.gameObject);
        }

        skinsViews = _scrollSnap.Content.GetComponentsInChildren<SkinView>().ToList();

        for (int i = 0; i < skinsViews.Count; i++)
        {
            skinsViews[i].Init(skins.ElementAt(i));
            skinsViews[i].Selected += OnSkinsViewSelected;
            skinsViews[i].Purchased += OnSkinsViewPurchased;
        }
        return skinsViews;
    }

    private void ClearViews(List<SkinView> skinsViews)
    {
        foreach (var skinView in skinsViews)
        {
            skinView.Selected -= OnSkinsViewSelected;
            skinView.Purchased -= OnSkinsViewPurchased;
            _scrollSnap.Remove(0);
        }
        skinsViews.Clear();
    }

    private void OnPanelCentered(int index)
    {
        Skin centeredSkin = _skinsViews[index].Skin;
        _bayButton.SetState(centeredSkin.IsPurchased, centeredSkin.IsReadyForSales);
        _bayButton.SetPriceText(centeredSkin.SkinData.Price);
        SetSelectButtonState(centeredSkin.IsSelected, centeredSkin.IsPurchased);

        if (_lastCenteredPanelIndex == index)
            return;

        _abilitiesInfoDisplay.SetDescription(centeredSkin);
        _skinsSelector.Visualizer.ShowSkin(centeredSkin);

        _isWaitingCentereView = false;
        _lastCenteredPanelIndex = index;
    }

    private void OnDragEneded()
    {
        _isWaitingCentereView = true;
    }

    private void OnSkinsSetted()
    {
        ClearViews(_skinsViews);
        _skinsViews = CreateListViews(_skinsSelector.Skins);
    }

    private void OnSkinsViewSelected(Skin skin)
    {
        _skinsSelector.SelectSkin(skin);
        _abilitiesInfoDisplay.SetDescription(skin);
        CentreSkinView(skin);
        SetSelectButtonState(skin.IsSelected, skin.IsPurchased);
    }

    private void CentreSkinView(Skin skin)
    {
        int indexPanel = 0;
        for (int i = 0; i < _skinsViews.Count; i++)
        {
            if (_skinsViews[i].Skin == skin)
            {
                indexPanel = i;
            }
        }
        _scrollSnap.GoToPanel(indexPanel);
    }

    private void OnSkinsViewPurchased(Skin skin)
    {
        _skinsSelector.PurchaseSkin(skin);
        _bayButton.SetState(skin.IsPurchased, skin.IsReadyForSales);
    }

    private void OnOpenButtonClick()
    {
#if !UNITY_EDITOR
        _leaderboardView.CloseLiders();
#endif
        _display.SetActive(true);
        CentreSkinView(_skinsSelector.CurrentSkin);
        _skinsPreviewer.Open();
        _player.PlayerMover.SetState(false);
    }

    public void OnCloseButtonClick()
    {
        _isWaitingCentereView = false;
        _skinsSelector.Visualizer.ShowSkin(_skinsSelector.CurrentSkin);
        _display.SetActive(false);
        _skinsPreviewer.Close();
        _player.PlayerMover.SetState(true);
    }

    public void CloseSkins()
    {
        OnCloseButtonClick();
    }

    public void DisableButton()
    {
        ChangedStatusButton(false);

        _iconTemplate.SetActive(true);
    }

    public void EnableButton()
    {
        ChangedStatusButton(true);

        _iconTemplate.SetActive(false);
    }

    public void ChangedStatusButton(bool result)
    {
        _openButton.interactable = result;

        for (int i = 0; i < _icons.Length; i++)
        {
            _icons[i].gameObject.SetActive(result);
        }
    }
    
    private void SetSelectButtonState(bool IsSelected, bool IsPurshed)
    {
        if (IsPurshed == true && IsSelected == false)
            _selectButton.gameObject.SetActive(true);
        else
            _selectButton.gameObject.SetActive(false);
    }
}
