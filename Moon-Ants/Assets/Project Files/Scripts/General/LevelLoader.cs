using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevelLoad()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log(nextIndex);
        Debug.Log(SceneManager.sceneCountInBuildSettings);
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
            nextIndex = 1;
        SceneManager.LoadScene(nextIndex);
    }

    public void PreviosLevelLoad()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex -1;
        if (nextIndex <= 0)
            nextIndex = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(nextIndex);
    }
}
