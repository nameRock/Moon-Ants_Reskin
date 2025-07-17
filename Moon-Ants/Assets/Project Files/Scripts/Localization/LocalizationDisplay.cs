using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalizationDisplay : MonoBehaviour
{
    [SerializeField] private LocalizationData[] _localizationDatas;
    [SerializeField] private Data _data;
    [SerializeField] private Button _languageButton;
    [SerializeField] private Image _icon;

    private int _dataIndex = 0;

    public LocalizationData LocalizationData => _localizationDatas[_dataIndex];

    private void Awake()
    {
        foreach (var data in _localizationDatas)
        {
            data.Init();
        }
    }

    private void Start()
    {
        if (Data.IsSeted == true)
        {
            SetLocalization();
            return;
        }
        Data.Setted += OnDataSeted;
    }

    private void OnDataSeted()
    {
        SetLocalization();
        Data.Setted -= OnDataSeted;
    }

    private void SetLocalization()
    {
#if (YANDEX_GAMES || CRAZY_GAMES||GAME_DISTRIBUTION) && UNITY_WEBGL && !UNITY_EDITOR
        if (_data.GetLanguage()== Language.Default.ToString())
        {            
            StartCoroutine(WaitSetDefaultLocalizaton());
            return;
        }
#endif    
        SetTargetDataIndex(_data.GetLanguage());
        SetLanguage(_localizationDatas[_dataIndex]);
    }

    private void OnEnable()
    {
        _languageButton.onClick.AddListener(OnLanguageButtonClick);
    }

    private void OnDisable()
    {
        _languageButton.onClick.RemoveListener(OnLanguageButtonClick);
    }

    private void SetTargetDataIndex(string targetLanguage)
    {
        for (int i = 0; i < _localizationDatas.Length; i++)
        {
            if (_localizationDatas[i].Language.ToString() == targetLanguage)
            {
                _dataIndex = i;
                break;
            }
        }
    }

    private void OnLanguageButtonClick()
    {
        _dataIndex++;
        if (_dataIndex >= _localizationDatas.Length)
        {
            _dataIndex = 0;
        }
        SetLanguage(_localizationDatas[_dataIndex]);
    }

    private void SetLanguage(LocalizationData localizationData)
    {
        _icon.sprite = localizationData.Icon;
        Localization.SetLanguage(localizationData);
        _data.SetLanguage(localizationData.Language.ToString());
    }

#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
    private IEnumerator WaitSetDefaultLocalizaton()
    {
        while(Agava.YandexGames.YandexGamesSdk.IsInitialized == false)
        {
            yield return null;
        }
        string lang = Agava.YandexGames.YandexGamesSdk.Environment.i18n.lang;
        Language language = Language.English;

        switch (lang)
        {
            case "ru":
                language = Language.Russian;
                break;
            case "en":
                language = Language.English;
                break;
            case "tr":
                language = Language.Turkish;
                break;
        }

        SetTargetDataIndex(language.ToString());
        SetLanguage(_localizationDatas[_dataIndex]);
    }
#endif

#if (CRAZY_GAMES||GAME_DISTRIBUTION) && UNITY_WEBGL && !UNITY_EDITOR

    private IEnumerator WaitSetDefaultLocalizaton()
    {
        yield return null;
        var lang = Application.systemLanguage;
        Language language = Language.English;

        switch (lang)
        {
            case SystemLanguage.Russian:
                language = Language.Russian;
                break;
            case SystemLanguage.Turkish:
                language = Language.Turkish;
                break;
            case SystemLanguage.Chinese:
                language = Language.Chinese;
                break;  
            case SystemLanguage.ChineseSimplified:
                language = Language.Chinese;
                break; 
            case SystemLanguage.ChineseTraditional:
                language = Language.Chinese;
                break;
            case SystemLanguage.German:
                language = Language.German;
                break;
            case SystemLanguage.French:
                language = Language.French;
                break;
            case SystemLanguage.Italian:
                language = Language.Italian;
                break;
            case SystemLanguage.Spanish:
                language = Language.Spanish;
                break;
            default:
                language = Language.English;
                break;
        }

        SetTargetDataIndex(language.ToString());
        SetLanguage(_localizationDatas[_dataIndex]);
    }
#endif
    //Test for CrazyGames
    public void DeleteSaves()
    {
        GameSceneManager gameSceneManager = FindObjectOfType<GameSceneManager>();
        gameSceneManager.Reset();
    }   
}
