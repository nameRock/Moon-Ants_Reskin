using UnityEngine;
using UnityEngine.UI;
using Agava.WebUtility.Samples;

[RequireComponent(typeof(Button))]
public class SoundButton : MonoBehaviour
{
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private Sprite _soundOffSprite;
    [SerializeField] private Image _icon;
    [SerializeField] private Data _data;

    private SoundState _soundState = SoundState.On;
    private Button _soundToggleButton;
    private bool _isRewardPlayed = false;
    private bool _isBackGround = false;
    private int _volume = 1;

    private void Awake()
    {
        _soundToggleButton = GetComponent<Button>();
    }

    private void Start()
    {
        if (Data.IsSeted == true)
        {
            SetSavedData();
            return;
        }
        Data.Setted += OnDataSeted;
    }

    private void OnDataSeted()
    {
        SetSavedData();
        Data.Setted -= OnDataSeted;
    }

    private void SetSavedData()
    {
        if (_data.GetSoundVolume() == 0)
            ChangeSoundState();
    }

    private void OnEnable()
    {
        _soundToggleButton.onClick.AddListener(OnSoundButtonClick);
        WebBackgroundVolumeSeter.VolumeSeted += OnBackgroundVolumeSeted;
        WebSdk.ADPlayed += OnAdPlayer;
    }

    private void OnDisable()
    {
        _soundToggleButton.onClick.RemoveListener(OnSoundButtonClick);
        WebBackgroundVolumeSeter.VolumeSeted -= OnBackgroundVolumeSeted;
        WebSdk.ADPlayed -= OnAdPlayer;
    }

    private void OnSoundButtonClick()
    {
        ChangeSoundState();
    }

    private void ChangeSoundState()
    {
        if (_soundState == SoundState.On)
            ActivateState(SoundState.Off, _soundOffSprite, 0);
        else
            ActivateState(SoundState.On, _soundOnSprite, 1);
    }

    private void ActivateState(SoundState soundState, Sprite sprite, int volume)
    {
        _soundState = soundState;
        _icon.sprite = sprite;
        _volume = volume;
        AudioListener.volume = volume;
        _data.SetSoundVolume(volume);        
    }

    private void OnBackgroundVolumeSeted(bool isBackground)
    {
        _isBackGround = isBackground;
        ChangeTemporaryState(isBackground);
    }

    private void OnAdPlayer(bool isADPlayed)
    {
        _isRewardPlayed = isADPlayed;
        ChangeTemporaryState(isADPlayed);
    }

    private void ChangeTemporaryState(bool isBackground)
    {
        if (isBackground == true)
        {
            AudioListener.volume = 0;
        }
        if (_isRewardPlayed == false && _isBackGround == false)
        {
            AudioListener.volume = _volume;
        }
    }

    private enum SoundState
    {
        On,
        Off
    }
}
