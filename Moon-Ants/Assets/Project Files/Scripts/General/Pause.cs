using Agava.WebUtility.Samples;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool _isADPlayed = false;
    private bool _isBackGround = false;
    private bool _isStoryPlayed = false;

    private void OnEnable()
    {
        WebBackgroundVolumeSeter.VolumeSeted += OnBackgroundVolumeSeted;
        WebSdk.ADPlayed += OnAdPlayer;
        StoryDisplay.StoryPlayed += OnStoryPlayed;
    }

    private void OnDisable()
    {
        WebBackgroundVolumeSeter.VolumeSeted -= OnBackgroundVolumeSeted;
        WebSdk.ADPlayed -= OnAdPlayer;
        StoryDisplay.StoryPlayed -= OnStoryPlayed;
    }

    private void OnBackgroundVolumeSeted(bool isActive)
    {
        _isBackGround = isActive;
        ChangeState();
    }

    private void OnAdPlayer(bool isActive)
    {
        _isADPlayed = isActive;
        ChangeState();
    }

    private void OnStoryPlayed(bool isActive)
    {
        _isStoryPlayed = isActive;
        ChangeState();
    }

    private void ChangeState()
    {
        if (_isBackGround == false && _isADPlayed == false && _isStoryPlayed == false)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }
}
