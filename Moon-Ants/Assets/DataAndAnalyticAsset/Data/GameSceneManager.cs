using Agava.YandexMetrica;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private Data _data;
    [SerializeField] private GaAnalytics _gaAnalytics;

    private Saver _saver;
    private int _nextLevelIndex;

    private void Awake()
    {
        _saver = _data.GetComponent<Saver>();
    }

    private void Start()
    {
#if YANDEX_GAMES && !UNITY_EDITOR
        YandexMetrica.Send("level" + SceneManager.GetActiveScene().buildIndex + "Start");
#endif

        StartCoroutine(WaitSave());

        _gaAnalytics.StartLevel(_data.GetDisplayedLevelNumber());
    }

    private void OnApplicationQuit()
    {
        _saver.Save();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            _saver.Save();
        }
    }

    public void SpendSoft(string purchaseType, string storeName, int purchaseAmount, int purchasesCount)
    {
        //_analytic.SendEventOnSoftSpend(purchaseType, storeName, purchaseAmount, purchasesCount);
    }

    public void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
            _nextLevelIndex = 4;
        else
            _nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        _data.SetLevelIndex(_nextLevelIndex);
        _gaAnalytics.CompliteLevel(_data.GetDisplayedLevelNumber());

        _data.AddDisplayedLevelNumber();

        _data.ResetTextRocket();            //////Test
        _data.SetCurrentIndexRocket(0);     //////Test
        Data.ClearItemSavedDatas();
        Data.ClearZonasSavedDatas();

        SaveAndLoadLevel();
    }

    public void LoadScene(int scenesIndex)
    {
        _data.SetLevelIndex(scenesIndex);
        _data.AddDisplayedLevelNumber();
        _data.ResetTextRocket();            
        _data.SetCurrentIndexRocket(0);
        Data.ClearItemSavedDatas();
        Data.ClearZonasSavedDatas();

        SaveAndLoadLevel();
    }

    public void Reset()
    {
        _saver.RemoveData();
        SaveAndLoadLevel();
    }

    private void SaveAndLoadLevel()
    {
        _saver.Saved += LoadScene;
        _saver.Save();
    }

    private void LoadScene()
    {
        _saver.Saved -= LoadScene;
        SceneManager.LoadScene(_data.GetLevelIndex());
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator WaitSave()
    {
        float delay = 15f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
        while (true)
        {
            yield return waitForSeconds;
            _saver.Save();
        }
    }
}
