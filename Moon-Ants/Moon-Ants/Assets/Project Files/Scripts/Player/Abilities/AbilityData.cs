using UnityEngine;

public abstract  class AbilityData : ScriptableObject
{
    [SerializeField] private LocalizationID _localizationID;
    [SerializeField] private string _additionalDescription;

    public LocalizationID LocalizationID => _localizationID;
    public string AdditionalDescription => _additionalDescription;

    
    public abstract void Activate(Player player);
    public abstract void Deactivate(Player player);
}
