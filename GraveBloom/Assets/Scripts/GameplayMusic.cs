using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayMusic : MonoBehaviour
{
    private static GameplayMusic instance;

    void Awake()
    {
        // ako gameplay muzika vec postoji iz prethodnog levela
        // unisti ovu novu kopiju
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
        //kad se vratimo u main menu,
        // gameplay muzika vise nije potrebna meni ima svoju
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
