using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject[] enemyPrefabs;
    public int totalEnemies = 15;
    public int maxAliveEnemies = 5;
    public float spawnInterval = 2f;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("UI")]
    public Image progressFill;
    public TMP_Text progressText;

    [Header("Level Exit")]
    public GameObject exitPointPrefab;
    public Transform exitSpawnPoint;
    public ExitIndicator exitIndicator;

    [Header("Next Level")]
    public string nextSceneName;

    private int spawnedEnemies = 0;
    private int killedEnemies = 0;
    private int aliveEnemies = 0;

    [Header("Final Level")]
    public bool isFinalLevel = false;
    public GameCompleteUI gameCompleteUI;
    public EnemyHealth bossHealth;
    public float gameCompleteDelay = 1.2f;

    private bool gameCompleteStarted = false;

    private Camera mainCamera;
    private GameObject spawnedExitPoint;

    void Start()
    {
        mainCamera = Camera.main;

        UpdateProgress();
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        while (spawnedEnemies < totalEnemies)
        {
            if (aliveEnemies < maxAliveEnemies)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = GetOffscreenSpawnPoint();

        if (spawnPoint == null)
            return;

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            return;

        GameObject prefab =
            enemyPrefabs[
                Random.Range(0, enemyPrefabs.Length)
            ];

        GameObject enemy =
            Instantiate(
                prefab,
                spawnPoint.position,
                Quaternion.identity
            );

        EnemyHealth health =
            enemy.GetComponentInChildren<EnemyHealth>();

        if (health != null)
        {
            health.SetWaveManager(this);
        }
        else
        {
            Debug.LogError(
                "SPAWNED ENEMY HAS NO EnemyHealth: " +
                enemy.name
            );
        }

        spawnedEnemies++;
        aliveEnemies++;
    }

    Transform GetOffscreenSpawnPoint()
    {
        List<Transform> availablePoints =
            new List<Transform>();

        foreach (Transform point in spawnPoints)
        {
            if (point == null)
                continue;

            Vector3 viewportPosition =
                mainCamera.WorldToViewportPoint(
                    point.position
                );

            bool visible =
                viewportPosition.z > 0 &&
                viewportPosition.x > 0 &&
                viewportPosition.x < 1 &&
                viewportPosition.y > 0 &&
                viewportPosition.y < 1;

            if (!visible)
            {
                availablePoints.Add(point);
            }
        }

        if (availablePoints.Count == 0)
            return null;

        return availablePoints[
            Random.Range(
                0,
                availablePoints.Count
            )
        ];
    }

    public void EnemyKilled()
    {
        killedEnemies++;
        aliveEnemies--;

        if (aliveEnemies < 0)
            aliveEnemies = 0;

        UpdateProgress();

        if (killedEnemies >= totalEnemies)
        {
            LevelComplete();
        }
    }

    void UpdateProgress()
    {
        if (progressFill != null)
        {
            progressFill.fillAmount =
                (float)killedEnemies /
                totalEnemies;
        }

        if (progressText != null)
        {
            progressText.text =
                killedEnemies +
                " / " +
                totalEnemies;
        }
    }

    void LevelComplete()
    {
        Debug.Log("LEVEL COMPLETE!");

        // Ako je poslednji level - prikaži završni ekran
        if (isFinalLevel)
        {
            if (gameCompleteUI != null)
            {
                gameCompleteUI.Show();
            }
            else
            {
                Debug.LogError(
                    "Final level nema povezan GameCompleteUI!"
                );
            }

            return;
        }

        // Obični leveli spawn-uju exit
        if (spawnedExitPoint != null)
            return;

        if (exitPointPrefab == null ||
            exitSpawnPoint == null)
        {
            Debug.LogError(
                "Exit Point Prefab ili Exit Spawn Point nisu povezani!"
            );

            return;
        }

        spawnedExitPoint =
            Instantiate(
                exitPointPrefab,
                exitSpawnPoint.position,
                Quaternion.identity
            );

        LevelExitPoint exit =
            spawnedExitPoint.GetComponent<LevelExitPoint>();

        if (exit != null)
        {
            exit.nextSceneName = nextSceneName;
        }

        if (exitIndicator != null)
        {
            exitIndicator.SetTarget(
                spawnedExitPoint.transform
            );
        }
    }
    void Update()
    {
        if (isFinalLevel && !gameCompleteStarted)
        {
            CheckFinalLevelComplete();
        }
    }

    void CheckFinalLevelComplete()
    {
        if (bossHealth == null)
            return;

        bool waveDead = killedEnemies >= totalEnemies;
        bool bossDead = bossHealth.IsDead();

        if (waveDead && bossDead)
        {
            gameCompleteStarted = true;
            StartCoroutine(ShowGameComplete());
        }
    }

    IEnumerator ShowGameComplete()
    {
        // malo sačekamo da se vidi boss death animacija
        yield return new WaitForSeconds(gameCompleteDelay);

        if (gameCompleteUI != null)
        {
            gameCompleteUI.Show();
        }
        else
        {
            Debug.LogError("Game Complete UI nije povezan!");
        }
    }
}