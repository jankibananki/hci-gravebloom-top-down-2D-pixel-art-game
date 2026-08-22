using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompleteUI : MonoBehaviour
{
    private const string LastLevelKey = "LastLevel";

    public void Show()
    {
        // igra je zavrsena brise se poslednji level
        PlayerPrefs.DeleteKey(LastLevelKey);
        PlayerPrefs.Save();

        gameObject.SetActive(true);

        Time.timeScale = 0f;

        Debug.Log("GAME COMPLETED - save reset.");
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;

        Debug.Log("Exit game.");
        Application.Quit();
    }
}
