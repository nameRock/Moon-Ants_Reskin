using System;

[System.Serializable]
public class Skin
{
    public SkinData SkinData { get; private set; }
    public bool IsPurchased { get; private set; } = false;
    public bool IsSelected { get; private set; } = false;
    public bool IsReadyForSales { get; private set; } = false;

    public event Action Selected;
    public event Action Unselected;
    public event Action Purchased;
    public event Action ReadyForSales;

    public Skin(SkinData skinData)
    {
        SkinData = skinData;
    }

    public void SetSavedData(SkinSavedData savedData)
    {
        if (savedData.IsSelected == true && savedData.IsPurchased == false)
            throw new ArgumentException("Not correct saved Data");

        IsPurchased = savedData.IsPurchased;
        SetSelectedStatus(savedData.IsSelected);
    }

    public void Select()
    {
        SetSelectedStatus(true);
    }

    public void Unselect()
    {
        SetSelectedStatus(false);
    }

    public void Purchase(Skin skin)
    {
        IsPurchased = true;
        IsReadyForSales = false;

        Purchased?.Invoke();
    }

    public void SetSaleStatus(int redCount)
    {
        if (IsPurchased == true)
            return;

        if (redCount >= SkinData.Price)
        {
            IsReadyForSales = true;
            ReadyForSales?.Invoke();
            return;
        }
        IsReadyForSales = false;
    }

    private void SetSelectedStatus(bool isSelected)
    {
        IsSelected = isSelected;

        if (isSelected == true)
            Selected?.Invoke();
        else
            Unselected?.Invoke();
    }
}
