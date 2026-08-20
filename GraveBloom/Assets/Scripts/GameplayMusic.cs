using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayMusic : MonoBehaviour
{
    private static GameplayMusic instance;

    void Awake()
    {
        // Ako gameplay muzika već postoji iz prethodnog levela,
        // uništi ovu novu kopiju.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Kad se vratimo u Main Menu,
        // gameplay muzika više nije potrebna.
        if (scene.name == "MainMenu")
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (instance == this)
            instance = null;
    }
}