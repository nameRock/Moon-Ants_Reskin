using UnityEngine;

[CreateAssetMenu(menuName = "SkinData")]
public class SkinData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _price = 0;
    [SerializeField] private Sprite _icon;
    [SerializeField] private AbilityData[] _abilitiesDatas;

    public string Name => _name;
    public string Description => _description;
    public int Price => _price;
    public Sprite Icon => _icon;
    public AbilityData[] AbilitiesDatas => _abilitiesDatas;

    public void ActivateAbilities(Player player)
    {
        if (_abilitiesDatas != null)
        {
            foreach (var data in _abilitiesDatas)
            {
                data.Activate(player);
            }
        }
    }

    public void DeactivateAbilities(Player player)
    {
        if (_abilitiesDatas != null)
        {
            foreach (var data in _abilitiesDatas)
            {
                data.Deactivate(player);
            }
        }
    }
}