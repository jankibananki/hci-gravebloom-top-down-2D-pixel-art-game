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

    // Koliko maksimalno može da bude živo odjednom
    public int maxAliveEnemies = 5;

    public float spawnInterval = 2f;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("UI")]
    public Image progressFill;
    public TMP_Text progressText;

    private int spawnedEnemies = 0;
    private int killedEnemies = 0;
    private int aliveEnemies = 0;

    private Camera mainCamera;

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

        if (enemyPrefabs.Length == 0)
            return;

        // Random enemy tip
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
            enemy.GetComponent<EnemyHealth>();

        if (health != null)
        {
            health.SetWaveManager(this);
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

            // Uzimamo samo pointove
            // koje kamera trenutno NE vidi
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

        // OVDE POSLE:
        // otvori kapiju
        // prikaži Level Complete UI
        // aktiviraj portal
        // prebaci na sledeći nivo
    }
}