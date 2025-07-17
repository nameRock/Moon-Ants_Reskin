using System;
using UnityEngine;

public class Localization : MonoBehaviour
{
    public static LocalizationData TranslationData { get; private set; }
    public static event Action<LocalizationData> LaguageChanged;

    public static void SetLanguage(LocalizationData translationData)
    {
        TranslationData = translationData;
        LaguageChanged?.Invoke(translationData);
    }
}

public enum Language
{
    Default,
    Russian,
    English,
    Turkish,
    Chinese,
    German,
    French,
    Spanish,
    Italian
}

[System.Serializable]
public class LocalizationDictionaty
{
    [SerializeField] private LocalizationID _iD;
    [SerializeField] private string _translation;

    public LocalizationID ID => _iD;
    public string Transition => _translation;
}

public enum LocalizationID
{
    Assistants,
    AssistantsStrength,
    AssistantsSpeed,
    AssistantsWorkers,
    Player,
    PlayerUpgrade,
    PlayerCapacity,
    PlayerSpeed,
    PlayerPrice,
    Leaderboard,
    EnemyDanger,
    Level,
    InviteFrieds,
    Next,
    YouWin,
    Apply,
    Select,
    AbilityDamage,
    AbilityRevenue,
    AbilitySpeed,
    AbilityStack,
    RewardSpeed,
    RewardIncome,
    RewardAntCount,
    NotInitialized,
    Danger,
    MaxStack,
    AuthorizedButton,
    StoryLevel1,
    StoryLevel2,
    StoryLevel3,
    StoryLevel4,
    StoryLevel5,
    StoryLevel6,
    StoryLevel7,
    StoryLevel8,
    StoryLevel9,
    StoryLevel10,
    StoryLevel11,
    StoryLevel12,
    Go,
    StoryLevel13,
    AdBlockDetected,
    Reward,
    YouReceive,
    See
}
