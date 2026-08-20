using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject continuePanel;

    [Header("Levels")]
    [SerializeField] private string firstLevelScene = "Level1";

    private const string LastLevelKey = "LastLevel";

    void Start()
    {
        Time.timeScale = 1f;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (continuePanel != null)
            continuePanel.SetActive(false);
    }

    // START BUTTON
    public void StartGame()
    {
        if (continuePanel != null)
            continuePanel.SetActive(true);
    }

    // YES - nastavi od poslednjeg levela
    public void ContinueGame()
    {
        string savedLevel = PlayerPrefs.GetString(
            LastLevelKey,
            firstLevelScene
        );

        Debug.Log("LOADING SAVED LEVEL: " + savedLevel);

        if (!Application.CanStreamedLevelBeLoaded(savedLevel))
        {
            Debug.LogWarning("Saved scene doesn't exist: " + savedLevel);
            savedLevel = firstLevelScene;
        }

        SceneManager.LoadScene(savedLevel);
    }

    // NO - kreni od početka
    public void NewGame()
    {
        PlayerPrefs.SetString(
            LastLevelKey,
            firstLevelScene
        );

        PlayerPrefs.Save();

        SceneManager.LoadScene(firstLevelScene);
    }

    public void CloseContinuePanel()
    {
        if (continuePanel != null)
            continuePanel.SetActive(false);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Exit button clicked.");
        Application.Quit();
    }
}