using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public CanvasGroup fadeOverlay;

    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;

    private bool isTransitioning = false;

    void Start()
    {
        // Svaki level počinje iz crnog
        fadeOverlay.alpha = 1f;
        fadeOverlay.blocksRaycasts = true;

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float time = 0f;

        while (time < fadeInDuration)
        {
            time += Time.unscaledDeltaTime;

            fadeOverlay.alpha =
                Mathf.Lerp(
                    1f,
                    0f,
                    time / fadeInDuration
                );

            yield return null;
        }

        fadeOverlay.alpha = 0f;
        fadeOverlay.blocksRaycasts = false;
    }

    public void LoadScene(string sceneName)
    {
        if (isTransitioning)
            return;

        StartCoroutine(
            LoadSceneRoutine(sceneName)
        );
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        isTransitioning = true;

        fadeOverlay.blocksRaycasts = true;

        float time = 0f;

        while (time < fadeOutDuration)
        {
            time += Time.unscaledDeltaTime;

            fadeOverlay.alpha =
                Mathf.Lerp(
                    0f,
                    1f,
                    time / fadeOutDuration
                );

            yield return null;
        }

        fadeOverlay.alpha = 1f;

        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }
}