using Agava.YandexGames;
using System;
using UnityEngine;

public class Saver : MonoBehaviour
{
    protected const string _dataKeyName = "MoonAnts";

    [SerializeField] private Data _data;
    [SerializeField] private WebSdk _webSdk;

    private string _lastSavedParameters;

    public Action Saved;
    public Action Loaded;

#if YANDEX_GAMES
    private void OnEnable()
    {
        _webSdk.Initialized += OnWebInitialized;
        _data.SaveNeed += OnDataSaveNeed;
    }

    private void OnDisable()
    {
        _webSdk.Initialized -= OnWebInitialized;
        _data.SaveNeed -= OnDataSaveNeed;
    }
#endif

#if UNITY_EDITOR || CRAZY_GAMES||GAME_DISTRIBUTION
    private void Start()
    {
        LoadLocalData();
    }
#endif

    public void Save()
    {
        _lastSavedParameters = JsonUtility.ToJson(Data.Parameters);
        SetPlayerSave(_lastSavedParameters);
    }

    public void Load()
    {
#if UNITY_EDITOR || CRAZY_GAMES||GAME_DISTRIBUTION
        LoadLocalData();
        return;
#endif
        PlayerAccount.GetPlayerData(OnDataGeted, OnGettingDataErrored);
    }

    [ContextMenu("RemoveData")]
    public void RemoveData()
    {
        PlayerPrefs.DeleteKey(Data.KeyName);
        _data.Clear();
        PlayerPrefs.DeleteAll();
    }

    private void LoadData(string data)
    {
        SaveParameters parameters = JsonUtility.FromJson<SaveParameters>(data);
        if (parameters == null)
        {
            Debug.Log("parameters == null ");
        }
        _data.SetParameters(parameters);
        Loaded?.Invoke();
    }

    private void LoadLocalData()
    {
        if (PlayerPrefs.HasKey(Data.KeyName) == false)
        {
            _data.CreateNew();
            Save();
        }

        LoadData(PlayerPrefs.GetString(Data.KeyName));
    }

    private void OnWebInitialized()
    {
        Load();
    }

    private void OnDataGeted(string data)
    {
        LoadData(data);
    }

    private void OnGettingDataErrored(string obj)
    {
        LoadLocalData();
    }

    private void SetPlayerSave(string saveData)
    {
#if UNITY_EDITOR|| CRAZY_GAMES||GAME_DISTRIBUTION
        SaveLocalData();
        return;
#endif
        PlayerAccount.SetPlayerData(saveData, OnDataSetted, OnSettingErrored);
    }

    private void OnDataSetted()
    {
        Saved?.Invoke();
    }

    private void OnSettingErrored(string obj)
    {
        SaveLocalData();
    }

    private void SaveLocalData()
    {
        PlayerPrefs.SetString(Data.KeyName, _lastSavedParameters);
        PlayerPrefs.Save();
        Saved?.Invoke();
    }

    private void OnDataSaveNeed()
    {
        Save();
    }
}
