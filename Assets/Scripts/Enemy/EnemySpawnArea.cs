using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Components")]
    public GameObject indicatorPrefab;
    public BoxCollider2D spawnArea;

    [Header("Prefabs (0:Default, 1:Fast, 2:Tank)")]
    public GameObject[] enemyPrefabs;

    [Header("Settings")]
    public float spawnInterval = 2f;
    public float warningDuration = 1.2f;
    public int maxEnemies = 10;

    private float timer;
    public int currentEnemyCount;

    void Update()
    {
        if (!GameManager.GameStarted) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval && currentEnemyCount < maxEnemies)
        {
            StartCoroutine(SpawnSequence());
            timer = 0f;
        }
    }

    IEnumerator SpawnSequence()
    {
        Vector2 spawnPos = GetRandomPointInBounds(spawnArea.bounds);
        GameObject indicator = Instantiate(indicatorPrefab, spawnPos, Quaternion.identity);

        yield return new WaitForSeconds(warningDuration);

        if (indicator != null) Destroy(indicator);
        SpawnEnemy(spawnPos);
    }

    void SpawnEnemy(Vector2 pos)
    {
        int wave = WaveManager.Instance != null ? WaveManager.Instance.currentWave : 1;
        GameObject prefab = GetEnemyByProbability(wave);

        GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);
        currentEnemyCount++;

        // Statlarý dalgaya göre ayarla
        if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth h))
        {
            h.SetSpawner(this);
            h.ScaleStats(wave);
        }
        if (enemy.TryGetComponent<EnemyFollow>(out EnemyFollow f))
        {
            f.ScaleSpeed(wave);
        }
    }

    GameObject GetEnemyByProbability(int wave)
    {
        float r = Random.value; // 0.0 - 1.0 arasý

        if (wave <= 1) return enemyPrefabs[0]; // 1. Dalga sadece Default

        if (wave < 5) // 2-4 Dalga: %80 Default, %20 Fast
        {
            return (r < 0.8f) ? enemyPrefabs[0] : enemyPrefabs[1];
        }

        // 5+ Dalga: %60 Default, %30 Fast, %10 Tank
        if (r < 0.6f) return enemyPrefabs[0];
        if (r < 0.9f) return enemyPrefabs[1];
        return enemyPrefabs[2];
    }

    Vector2 GetRandomPointInBounds(Bounds b)
    {
        return new Vector2(Random.Range(b.min.x, b.max.x), Random.Range(b.min.y, b.max.y));
    }

    public void OnEnemyDestroyed() => currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
}