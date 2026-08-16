using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup gameOverPanel;
    public CanvasGroup fadeOverlay;

    [Header("Timing")]
    public float deathDelay = 1f;
    public float popupFadeDuration = 0.4f;
    public float sceneFadeDuration = 0.5f;

    [Header("Scenes")]
    public string mainMenuScene = "MainMenu";

    private bool gameOverShown = false;

    void Awake()
    {
        gameOverPanel.alpha = 0f;
        gameOverPanel.interactable = false;
        gameOverPanel.blocksRaycasts = false;

        fadeOverlay.alpha = 0f;
        fadeOverlay.interactable = false;
        fadeOverlay.blocksRaycasts = false;
    }

    public void ShowGameOver()
    {
        if (gameOverShown)
            return;

        gameOverShown = true;

        StartCoroutine(ShowGameOverRoutine());
    }

    IEnumerator ShowGameOverRoutine()
    {
        // Pusti prvo death animaciju
        yield return new WaitForSeconds(deathDelay);

        // Zamrzni gameplay
        Time.timeScale = 0f;

        gameOverPanel.blocksRaycasts = true;

        float time = 0f;

        while (time < popupFadeDuration)
        {
            time += Time.unscaledDeltaTime;

            gameOverPanel.alpha =
                Mathf.Lerp(
                    0f,
                    1f,
                    time / popupFadeDuration
                );

            yield return null;
        }

        gameOverPanel.alpha = 1f;
        gameOverPanel.interactable = true;
    }

    // YES
    public void Retry()
    {
        StartCoroutine(
            LoadSceneWithFade(
                SceneManager.GetActiveScene().name
            )
        );
    }

    // NO
    public void BackToMenu()
    {
        StartCoroutine(
            LoadSceneWithFade(mainMenuScene)
        );
    }

    IEnumerator LoadSceneWithFade(string sceneName)
    {
        gameOverPanel.interactable = false;

        fadeOverlay.blocksRaycasts = true;

        float time = 0f;

        while (time < sceneFadeDuration)
        {
            time += Time.unscaledDeltaTime;

            fadeOverlay.alpha =
                Mathf.Lerp(
                    0f,
                    1f,
                    time / sceneFadeDuration
                );

            yield return null;
        }

        fadeOverlay.alpha = 1f;

        // Obavezno vrati vreme pre nove scene
        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }
}