using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private LocalizationID _localizationID;

    public TMP_Text Label { get; private set; }
    private LocalizationData _localizationData;
    private string _additionDescription = string.Empty;
    private TMP_FontAsset _defaultFont;

    protected LocalizationID LocalizationID => _localizationID;

    public event Action TextSetted;

    private void Awake()
    {
        Label = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if (Localization.TranslationData != null)
            SetText(Localization.TranslationData);

        Localization.LaguageChanged += OnLaguageChanged;
    }

    private void OnDisable()
    {
        Localization.LaguageChanged -= OnLaguageChanged;
    }

    public void SetLocalizationID(LocalizationID localizationID)
    {
        _localizationID = localizationID;

        if (Localization.TranslationData != null)
            SetText(Localization.TranslationData);
    }

    public void SetAdditionalDescription(string additionalDescription)
    {
        _additionDescription = additionalDescription;
    }

    private void OnLaguageChanged(LocalizationData data)
    {
        _localizationData = data;
        SetText(data);
    }

    protected virtual void SetText(LocalizationData data)
    {
        if (_defaultFont == null)
            _defaultFont = Label.font;
        if (data.Font == null)
            Label.font = _defaultFont;
        else
            Label.font = data.Font;

        Label.text = GetTranslation(data);
        TextSetted?.Invoke();
    }

    protected virtual string GetTranslation(LocalizationData data)
    {
        return data.GetTranslation(_localizationID);
    }
}
