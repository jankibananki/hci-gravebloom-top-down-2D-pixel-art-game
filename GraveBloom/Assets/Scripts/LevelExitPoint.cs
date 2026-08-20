using UnityEngine;

public class LevelExitPoint : MonoBehaviour
{
    [HideInInspector]
    public string nextSceneName;

    private bool used = false;
    private SceneTransition sceneTransition;

    void Start()
    {
        sceneTransition = FindAnyObjectByType<SceneTransition>();

        Debug.Log("EXIT POINT VODI NA: " + nextSceneName);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("LevelExitPoint nema postavljen nextSceneName!");
            return;
        }

        if (sceneTransition == null)
        {
            Debug.LogError("SceneTransition nije pronađen u sceni!");
            return;
        }

        used = true;

        Debug.Log("PRELAZIM NA: " + nextSceneName);

        sceneTransition.LoadScene(nextSceneName);
    }
}