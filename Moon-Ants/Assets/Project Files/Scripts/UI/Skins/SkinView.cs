using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinView : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Image _skinIcon;
    [SerializeField] private Image _selectedIcon;
    [SerializeField] private Image _lockIcon;
    [SerializeField] private Button _selectButton;
    [SerializeField] private BayButton _bayButton;

    private Skin _skin;

    public Skin Skin => _skin;

    public event Action<Skin> Selected;
    public event Action<Skin> Purchased;

    private void Start()
    {
        if (_skin.IsSelected == true)
            Selected?.Invoke(_skin);
    }

    private void OnEnable()
    {
        // _selectButton.onClick.AddListener(OnSelectButtonClick);
        // _bayButton.Clicked += OnBayButtonClick;
        _skin.Selected += OnSkinSelected;
        _skin.Unselected += OnSkinUnselected;
        _skin.Purchased += OnSkinPurchased;
        _skin.ReadyForSales += OnReadyForSales;
        SetSelectButtonState(_skin.IsSelected, _skin.IsPurchased);
    }

    private void OnDisable()
    {
        // _selectButton.onClick.RemoveListener(OnSelectButtonClick);
        // _bayButton.Clicked -= OnBayButtonClick;
        _skin.Selected -= OnSkinSelected;
        _skin.Unselected -= OnSkinUnselected;
        _skin.Purchased -= OnSkinPurchased;
        _skin.ReadyForSales -= OnReadyForSales;
    }

    public void Init(Skin skin)
    {
        _skin = skin;
        ShowSkin(_skin);        
        enabled = true;
    }

    private void ShowSkin(Skin skin)
    {
        _skinIcon.sprite = skin.SkinData.Icon;
        SetSelectButtonState(skin.IsSelected, skin.IsPurchased);
    }

    public void OnBayButtonClick()
    {
        if (_skin.IsReadyForSales == true)
            Purchased?.Invoke(_skin);
    }

    public void OnSelectButtonClick()
    {
        Selected?.Invoke(_skin);
    }
    
    private void SetSelectButtonState(bool IsSelected, bool IsPurshed)
    {
        _selectedIcon.gameObject.SetActive(IsSelected);
        _lockIcon.gameObject.SetActive(IsSelected == false && IsPurshed == false);
    }

    private void OnSkinSelected()
    {
        SetSelectButtonState(_skin.IsSelected, _skin.IsPurchased);
    }

    private void OnSkinUnselected()
    {
        SetSelectButtonState(_skin.IsSelected, _skin.IsPurchased);
    }

    private void OnSkinPurchased()
    {
        SetSelectButtonState(_skin.IsSelected, _skin.IsPurchased);
    }

    private void OnReadyForSales()
    {
    }
}
