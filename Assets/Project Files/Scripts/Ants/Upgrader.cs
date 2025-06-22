using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{
    [SerializeField] private int _startPrice;
    [SerializeField] private int _priceMultiplier;
    [SerializeField] private TMP_Text _textPrice;
    [SerializeField] private TMP_Text _textLevel;
    [SerializeField] private Button _button;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Color _redColor;
    [SerializeField] private Color _greenColor;
    [SerializeField] private CrystalsType _crystalsType = CrystalsType.Blue;
    [SerializeField] private GameSceneManager _sceneManager;
    [SerializeField] private Data _data;
    [SerializeField] private bool _isOverMultiplier = false;
    [SerializeField] private AudioSource _audioSource;

    private int _currentPrice;
    private int _currentLevel = 1;
    private bool _isActive;
    private bool _isIgnoringPayment = false;
    private Coroutine _detain;

    public int CurrentPrice => _currentPrice;
    public bool IsActive => _isActive;
    public Button Button => _button;

    private void Update()
    {
        int countValue = GetCountValue();

        if (countValue >= _currentPrice)
        {
            _button.interactable = true;
            _textPrice.color = _greenColor;
            _isActive = true;
        }
        else
        {
            _button.interactable = false;
            _textPrice.color = _redColor;
            _isActive = false;
        }
    }

    public void ChangedPriceValue()
    {
        _audioSource.Stop();
        _audioSource.Play();

        if (_data != null)
        {
            _data.SetNumberSoftSpend();
        }

        if (_sceneManager != null)
        {
            _sceneManager.SpendSoft("Upgrade", gameObject.name + " " + _currentLevel + "lvl", _currentPrice, _data.GetNumberSoftSpend());
        }

        if (_isIgnoringPayment == false)
        {
            SpendCristals();
        }

        if (_isOverMultiplier)
        {
            AddOverMultiplier();
        }

        _currentPrice += _priceMultiplier;
        _currentLevel++;

        ChangedTextPrice();
        _isIgnoringPayment = false;

        _button.interactable = false;
        _isActive = false;
    }

    private void AddOverMultiplier()
    {
        if (_currentLevel >= 100)
        {
            _priceMultiplier = 30;
        }
        else if (_currentLevel >= 60)
        {
            _priceMultiplier = 15;
        }
        else if (_currentLevel >= 30)
        {
            _priceMultiplier = 8;
        }
        else if (_currentLevel >= 15)
        {
            _priceMultiplier = 4;
        }
    }

    private void SpendCristals()
    {
        if (_crystalsType == CrystalsType.Blue)
        {
            _wallet.SpendBlueCristals(_currentPrice, "Buy", "Upgrade");
        }
        else if (_crystalsType == CrystalsType.Green)
        {
            _wallet.SpendGreenCristals(_currentPrice);
        }
    }

    public void GetLevelUpgrade(int level)
    {
        _currentLevel = level;
        if (_isOverMultiplier)
        {
            AddOverMultiplier();
        }

        _currentPrice = _startPrice + (_priceMultiplier * (_currentLevel - 1));

        ChangedTextPrice();
    }

    public void ChangedTextPrice()
    {
        _textPrice.text = _currentPrice.ToString();
        _textLevel.text = _currentLevel.ToString();
    }

    public void IgnorePayment() 
    {
        _isIgnoringPayment = true;
    }

    private int GetCountValue()
    {
        if (_crystalsType == CrystalsType.Blue)
            return _wallet.BlueCountValue;
        else
            return _wallet.GreenCountValue;
    }

    private enum CrystalsType
    {
        Blue,
        Green
    }
}


