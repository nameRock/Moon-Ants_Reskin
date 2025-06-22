using System;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public const string KeyName = "MoonAnts";

    public static SaveParameters Parameters { get; private set; } = new SaveParameters();

    public event Action SaveNeed;
    public static event Action Setted;
    public static bool IsSeted { get; private set; } = false;

    public void CreateNew()
    {
        Parameters = new SaveParameters();
    }

    public void SetParameters(SaveParameters parameters)
    {
        Parameters = parameters;
        Setted?.Invoke();
        IsSeted = true;
    }

    public void Clear()
    {
        Parameters = new SaveParameters();
    }

    public void SetNumberSoftSpend()
    {
        Parameters.NumberSoftSpend++;
    }

    public void SetLevelIndex(int index)
    {
        Parameters.LevelNumber = index;
    }

    public void SetDateRegistration(DateTime date)
    {
        Parameters.RegistrationDate = date.ToString();
    }

    public void SetLastLoginDate(DateTime date)
    {
        Parameters.LastLoginDate = date.ToString();
    }

    public void SetCurrentSoft(int value)
    {
        Parameters.Soft = value;
    }

    public void SetCurrentDopSoft(int value)
    {
        Parameters.DopSoft = value;
    }

    public void SetRedSoft(int redCountValue)
    {
        Parameters.RedSoft = redCountValue;
    }

    public void SetCountUpgrade()
    {
        Parameters.CountAntsUpgrade++;
    }

    public void SetSpeedUpgrade()
    {
        Parameters.SpeedAntsUpgrade++;
    }

    public void SetForceUpgrade()
    {
        Parameters.ForceAntsUpgrade++;
    }

    public void SetPlayerPriceUpgrade()
    {
        Parameters.PlayerPriceUpgrade++;
        //SaveNeed?.Invoke();
    }

    public void SetPlayerSpeedUpgrade()
    {
        Parameters.PlayerSpeedUpgrade++;
        //SaveNeed?.Invoke();
    }

    public void SetPlayerStackUpgrade()
    {
        Parameters.PlayerStackUpgrade++;
        //SaveNeed?.Invoke();
    }
    public void SetCurrentIndexRocket(int index)
    {
        Parameters.CurrentIndexRocket = index;
    }

    public void SetSkins(SkinSavedData[] skinsSavedDatas)
    {
        Parameters.SkinSavedDatas = skinsSavedDatas;
        SaveNeed?.Invoke();
    }

    public static void SetItemsSavedDatas(ItemSavedData itemSavedData)
    {
        for (int i = 0; i < Parameters.ItemSavedDatas.Count; i++)
        {
            if (Parameters.ItemSavedDatas[i].Name == itemSavedData.Name)
            {
                Parameters.ItemSavedDatas[i].IndexItem = itemSavedData.IndexItem;
                return;
            }
        }

        Parameters.ItemSavedDatas.Add(itemSavedData);
    }

    public static void SetSavedZona(SaveZona saveZona)
    {
        for (int i = 0; i < Parameters.SaveZonas.Count; i++)
        {
            if (Parameters.SaveZonas[i].Name == saveZona.Name)
            {
                Parameters.SaveZonas[i].IsOpen = saveZona.IsOpen;
                return;
            }
        }

        Parameters.SaveZonas.Add(saveZona);
    }

    public void SetIndexTextRocket()
    {
        Parameters.IndexTextRocket++;
    }

    public void SetSoundVolume(int volume)
    {
        Parameters.SoundVolume = volume;
        SaveNeed?.Invoke();
    }

    public void SetInterstitialADDelay(int delay)
    {
        Parameters.InterstitialADDelay = delay;
    }

    public void SetLanguage(string language)
    {
        Parameters.Language = language;
    }
    public void ResetTextRocket()
    {
        Parameters.IndexTextRocket = 0;
    }

    public void AddSession()
    {
        Parameters.SessionCount++;
    }

    public void SetIgnoreNextInterstialAd(bool isIgnore)
    {
        Parameters.IsIgnoreNextInterstitialAD = isIgnore;
    }

    public int GetNumberSoftSpend()
    {
        return Parameters.NumberSoftSpend;
    }

    public int GetIndexTextRocket()
    {
        return Parameters.IndexTextRocket;
    }

    public int GetIndexRocket()
    {
        return Parameters.CurrentIndexRocket;
    }

    public int GetCountUpgrade()
    {
        return Parameters.CountAntsUpgrade;
    }

    public int GetSpeedUpgrade()
    {
        return Parameters.SpeedAntsUpgrade;
    }

    public int GetForceUpgrade()
    {
        return Parameters.ForceAntsUpgrade;
    }
    public int GetPlayerPriceUpgrade()
    {
        return Parameters.PlayerPriceUpgrade;
    }

    public int GetPlayerSpeedUpgrade()
    {
        return Parameters.PlayerSpeedUpgrade;
    }

    public int GetPlayerStackUpgrade()
    {
        return Parameters.PlayerStackUpgrade;
    }

    public int GetLevelIndex()
    {
        return Parameters.LevelNumber;
    }

    public int GetSessionCount()
    {
        return Parameters.SessionCount;
    }

    public void AddDisplayedLevelNumber()
    {
        Parameters.DisplayedLevelNumber++;
    }

    public int GetNumberDaysAfterRegistration()
    {
        return (DateTime.Parse(Parameters.LastLoginDate) - DateTime.Parse(Parameters.RegistrationDate)).Days;
    }

    public int GetDisplayedLevelNumber()
    {
        return Parameters.DisplayedLevelNumber;
    }

    public string GetRegistrationDate()
    {
        return Parameters.RegistrationDate;
    }

    public int GetCurrentSoft()
    {
        return Parameters.Soft;
    }
    public int GetCurrentDopSoft()
    {
        return Parameters.DopSoft;
    }
    public int GetRedSoft()
    {
        return Parameters.RedSoft;
    }

    public int GetSoundVolume()
    {
        return Parameters.SoundVolume;
    }

    public string GetLanguage()
    {
        return Parameters.Language;
    }

    public int GetInterstitialADDelay()
    {
        return Parameters.InterstitialADDelay;
    }

    public bool GetIsIgnoreNextInterstitialAd()
    {
        return Parameters.IsIgnoreNextInterstitialAD;
    }

    public SkinSavedData[] GetSkins()
    {
        return Parameters.SkinSavedDatas;
    }

    public static ItemSavedData GetItemSavedDatas(ItemSavedData itemSavedData)
    {
        foreach (var data in Parameters.ItemSavedDatas)
        {
            if (data.Name == itemSavedData.Name)
                return data;
        }
        return null;
    }

    public static SaveZona GetSavedZona(SaveZona saveZona)
    {
        foreach (var zona in Parameters.SaveZonas)
        {
            if (zona.Name == saveZona.Name)
                return zona;
        }
        return null;
    }

    public static void ClearItemSavedDatas()
    {
        Parameters.ItemSavedDatas.Clear();
    }

    public static void ClearZonasSavedDatas()
    {
        Parameters.SaveZonas.Clear();
    }
}

[Serializable]
public class SaveParameters
{
    public int LevelNumber = 1;
    public int CurrentIndexRocket = 0;
    public int CountAntsUpgrade = 1;
    public int SpeedAntsUpgrade = 1;
    public int ForceAntsUpgrade = 1;
    public int PlayerSpeedUpgrade = 1;
    public int PlayerPriceUpgrade = 1;
    public int PlayerStackUpgrade = 1;
    public int SessionCount = 0;
    public string LastLoginDate = DateTime.Now.ToString();
    public string RegistrationDate = DateTime.Now.ToString();
    public int DisplayedLevelNumber = 1;
    public int Soft = 0;
    public int DopSoft = 0;
    public int RedSoft = 0;
    public int IndexTextRocket;
    public int NumberSoftSpend = 0;
    public int SoundVolume = 1;
    public int InterstitialADDelay = 120;
    public string Language = "Default";
    public SkinSavedData[] SkinSavedDatas = new SkinSavedData[0];
    public List<ItemSavedData> ItemSavedDatas = new List<ItemSavedData>();
    public List<SaveZona> SaveZonas = new List<SaveZona>();
    public bool IsIgnoreNextInterstitialAD = false;
}