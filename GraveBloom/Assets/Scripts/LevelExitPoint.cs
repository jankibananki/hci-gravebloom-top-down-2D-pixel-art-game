using UnityEngine;

public class LevelExitPoint : MonoBehaviour
{
    public string nextSceneName;

    private bool used = false;

    private SceneTransition sceneTransition;

    void Start()
    {
        sceneTransition =
            FindAnyObjectByType<SceneTransition>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used)
            return;

        if (!other.CompareTag("Player"))
            return;

        used = true;

        if (sceneTransition != null)
        {
            sceneTransition.LoadScene(
                nextSceneName
            );
        }
    }
}