using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilitiesTextsDisplay : MonoBehaviour
{
    [SerializeField] private LocalizedText _descriptionPrefabs;
    [SerializeField] private Transform _content;

    private List<LocalizedText> _descriptionds = new List<LocalizedText>();

    private void OnDisable()
    {
        RemoveAllDescriptions();
    }

    public void SetDescription(Skin skin)
    {
        RemoveAllDescriptions();

        if (skin.SkinData.AbilitiesDatas == null)
            return;

        foreach (var data in skin.SkinData.AbilitiesDatas)
        {
            LocalizedText description = Instantiate(_descriptionPrefabs, _content);
            description.SetLocalizationID(data.LocalizationID);
            _descriptionds.Add(description);
        }    
    }

    private void RemoveAllDescriptions()
    {
        foreach (var description in _descriptionds)
        {
            Destroy(description.gameObject);
        }
        _descriptionds.Clear();
    }
}
