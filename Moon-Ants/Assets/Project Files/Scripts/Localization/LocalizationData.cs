using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New LocalizationData", menuName = "LocalizationData")]
public class LocalizationData : ScriptableObject
{
    [SerializeField] private Language _language;
    [SerializeField] private Sprite _icon;
    [SerializeField] private TMP_FontAsset _font;
    [SerializeField] private LocalizationDictionaty Assistants;
    [SerializeField] private LocalizationDictionaty AssistantsStrength;
    [SerializeField] private LocalizationDictionaty AssistantsSpeed;
    [SerializeField] private LocalizationDictionaty AssistantsWorkers;
    [SerializeField] private LocalizationDictionaty Player;
    [SerializeField] private LocalizationDictionaty PlayerUpgrade;
    [SerializeField] private LocalizationDictionaty PlayerCapacity;
    [SerializeField] private LocalizationDictionaty PlayerSpeed;
    [SerializeField] private LocalizationDictionaty PlayerPrice;
    [SerializeField] private LocalizationDictionaty Leaderboard;
    [SerializeField] private LocalizationDictionaty EnemyDanger;
    [SerializeField] private LocalizationDictionaty Level;
    [SerializeField] private LocalizationDictionaty InviteFrieds;
    [SerializeField] private LocalizationDictionaty YouWin;
    [SerializeField] private LocalizationDictionaty Next;
    [SerializeField] private LocalizationDictionaty Go;
    [SerializeField] private LocalizationDictionaty Apply;
    [SerializeField] private LocalizationDictionaty Select;
    [SerializeField] private LocalizationDictionaty AbilityDamage;
    [SerializeField] private LocalizationDictionaty AbilityRevenue;
    [SerializeField] private LocalizationDictionaty AbilitySpeed;
    [SerializeField] private LocalizationDictionaty AbilityStack;
    [SerializeField] private LocalizationDictionaty RewardSpeed;
    [SerializeField] private LocalizationDictionaty RewardIncome;
    [SerializeField] private LocalizationDictionaty RewardAntCount;
    [SerializeField] private LocalizationDictionaty NotInitialized;
    [SerializeField] private LocalizationDictionaty Danger;
    [SerializeField] private LocalizationDictionaty MaxStack;
    [SerializeField] private LocalizationDictionaty AuthorizedButton;
    [SerializeField] private LocalizationDictionaty StoryLevel1;
    [SerializeField] private LocalizationDictionaty StoryLevel2;
    [SerializeField] private LocalizationDictionaty StoryLevel3;
    [SerializeField] private LocalizationDictionaty StoryLevel4;
    [SerializeField] private LocalizationDictionaty StoryLevel5;
    [SerializeField] private LocalizationDictionaty StoryLevel6;
    [SerializeField] private LocalizationDictionaty StoryLevel7;
    [SerializeField] private LocalizationDictionaty StoryLevel8;
    [SerializeField] private LocalizationDictionaty StoryLevel9;
    [SerializeField] private LocalizationDictionaty StoryLevel10;
    [SerializeField] private LocalizationDictionaty StoryLevel11;
    [SerializeField] private LocalizationDictionaty StoryLevel12;
    [SerializeField] private LocalizationDictionaty StoryLevel13;
    [SerializeField] private LocalizationDictionaty AdBlockDetected;
    [SerializeField] private LocalizationDictionaty Reward;
    [SerializeField] private LocalizationDictionaty YouReceive;
    [SerializeField] private LocalizationDictionaty See;

    private HashSet<LocalizationDictionaty> _dictionaries;

    public Sprite Icon => _icon;
    public Language Language => _language;
    public TMP_FontAsset Font => _font;

    public void Init()
    {
        _dictionaries = new HashSet<LocalizationDictionaty>
        { Assistants, AssistantsStrength, AssistantsSpeed, AssistantsWorkers,Player, PlayerUpgrade,
          PlayerCapacity, PlayerSpeed,  PlayerPrice, Leaderboard, EnemyDanger, Level, InviteFrieds, YouWin, Next,Go, Apply,
          Select, AbilityDamage, AbilityRevenue, AbilitySpeed, AbilityStack, RewardSpeed, RewardIncome, RewardAntCount, NotInitialized, Danger,
          MaxStack, AuthorizedButton, StoryLevel1, StoryLevel2, StoryLevel3, StoryLevel4, StoryLevel5, StoryLevel6, StoryLevel7, StoryLevel8, StoryLevel9,
          StoryLevel10, StoryLevel11, StoryLevel12, StoryLevel13,AdBlockDetected, Reward, YouReceive, See};
    }

    public string GetTranslation(LocalizationID localizationID)
    {
        var targetDictionaty = _dictionaries.First(dictionary => dictionary.ID == localizationID);
        return targetDictionaty.Transition;
    }
}
