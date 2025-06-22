[System.Serializable]
public class SkinSavedData 
{
    public string Name;
    public bool IsPurchased;
    public bool IsSelected;

    public SkinSavedData(Skin skin)
    {
        Name = skin.SkinData.Name;
        IsPurchased = skin.IsPurchased;
        IsSelected = skin.IsSelected;
    }
}
