using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private TMP_Text _blueText;
    [SerializeField] private TMP_Text _greenText;
    [SerializeField] private TMP_Text _redText;
    [SerializeField] private Data _data;
    [SerializeField] private GaAnalytics _gaAnalytics;

    private int _blueCountValue = 0;
    private int _greenCountValue = 0;
    private int _redCountValue = 0;
    private int _priceMultiplier = 0;
    private int _abilityPriceStep = 0;
    private float _rewardPriceMultiplier = 1f;
    private int _waitingBlueCristal = 0;
    private Coroutine _waitSendBlueCristal;
    public int BlueCountValue => _blueCountValue;
    public int GreenCountValue => _greenCountValue;
    public int RedCountValue => _redCountValue;

    public event Action<int> RedCristallChanged;

    private void Start()
    {
        SetSavedData();
        if (Data.IsSeted == true)
        {
            SetSavedData();
            return;
        }
        Data.Setted += OnDataSeted;
    }

    private void OnDataSeted()
    {
        SetSavedData();
        Data.Setted -= OnDataSeted;
    }

    private void SetSavedData()
    {
        _blueCountValue = _data.GetCurrentSoft();
        _greenCountValue = _data.GetCurrentDopSoft();
        _redCountValue = _data.GetRedSoft();

#if UNITY_EDITOR
        Debug.Log("Money");
        _blueCountValue = 100000000;
        _greenCountValue = 100000000;
        _redCountValue = 50;
#endif
        ValueChanged(_blueText, _blueCountValue);
        ValueChanged(_greenText, _greenCountValue);
        RedCristallChanged?.Invoke(_redCountValue);

        if (_redText != null)
            ValueChanged(_redText, _redCountValue);
    }

    public void AddBlueCristals(int price, string source, string character)
    {
        int addedCristal = (int)Math.Ceiling((price + _priceMultiplier + _abilityPriceStep) * _rewardPriceMultiplier);
        _blueCountValue += addedCristal;
        _data.SetCurrentSoft(_blueCountValue);
        _waitingBlueCristal += addedCristal;
        _gaAnalytics.SourceValue("BlueCristal", character, source, (float)addedCristal);

        ValueChanged(_blueText, _blueCountValue);
    }

    public void AddGreenCristals(int count, string source)
    {
        _greenCountValue += count;
        _data.SetCurrentDopSoft(_greenCountValue);
        _gaAnalytics.SourceValue("GreenCristal", "Player", source, (float)count);

        ValueChanged(_greenText, _greenCountValue);
    }

    public void AddRedCristals(int count, string source)
    {
        _redCountValue += count;
        _data.SetRedSoft(_redCountValue);
        RedCristallChanged?.Invoke(_redCountValue);
        _gaAnalytics.SourceValue("RedCristal", "Player", source, (float)count);

        if (_redText != null)
            ValueChanged(_redText, _redCountValue);
    }

    public void SpendBlueCristals(int num, string type, string shop)
    {
        _blueCountValue -= num;
        _data.SetCurrentSoft(_blueCountValue);
        _gaAnalytics.SinkValue("BlueCristal", type, shop, (float)num);

        ValueChanged(_blueText, _blueCountValue);
    }

    public void SpendGreenCristals(int num)
    {
        _greenCountValue -= num;
        _data.SetCurrentDopSoft(_greenCountValue);
        _gaAnalytics.SinkValue("GreenCristal", "Buy", "Upgrade", (float)num);

        ValueChanged(_greenText, _greenCountValue);
    }

    public void SpendRedCristals(int num)
    {
        _redCountValue -= num;
        _data.SetRedSoft(_redCountValue);
        RedCristallChanged?.Invoke(_redCountValue);
        _gaAnalytics.SinkValue("RedCristal", "Buy", "Skins", (float)num);

        if (_redText != null)
            ValueChanged(_redText, _redCountValue);
    }

    public void ChangeMultiplier(int lvl)
    {
        _priceMultiplier += (lvl - 1);
    }

    public void SetAbilityPriceStep(int priceStep)
    {
        _abilityPriceStep = priceStep;
    }

    public void ActivatePriceBoost(int percent, float time)
    {
        _rewardPriceMultiplier = (float)percent / 100;
        StartCoroutine(WaitDeactivateBoost(time));
    }

    private IEnumerator WaitDeactivateBoost(float time)
    {
        yield return new WaitForSeconds(time);
        _rewardPriceMultiplier = 1;
    }

    private void ValueChanged(TMP_Text text, int value)
    {
        if (text != null)
        {
            text.text = value.ToString();
        }
    }  
}
