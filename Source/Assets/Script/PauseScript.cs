using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public void PauseGame()
    {
        Time.timeScale = 0;
        SceneManager.LoadSceneAsync("PauseScene", LoadSceneMode.Additive);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("PauseScene");
    }
}
