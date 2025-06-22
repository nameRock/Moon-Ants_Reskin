using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BayButton : MonoBehaviour
{
    [SerializeField] private Button _bayButton;
    [SerializeField] private TMP_Text _priceText;

    public event Action Clicked;

    private void OnEnable()
    {
        _bayButton.onClick.AddListener(() => Clicked?.Invoke());
    }

    private void OnDisable()
    {
        _bayButton.onClick.RemoveListener(() => Clicked?.Invoke());
    }

    public void SetState(bool IsPurchased, bool IsReadyForSales)
    {
        gameObject.SetActive(IsPurchased == false);

        if (IsPurchased == false)
            _bayButton.interactable = IsReadyForSales;
    }

    public void SetPriceText(int price)
    {
        _priceText.text = price.ToString();
    }
}
