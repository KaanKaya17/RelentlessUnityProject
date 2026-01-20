using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Components")]
    public GameObject indicatorPrefab;
    public BoxCollider2D spawnArea;
    public GameObject[] enemyPrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public float warningDuration = 1.5f; 
    public int maxEnemies = 10;

    private float timer;
    public int currentEnemyCount;

    void Start()
    {
        if (spawnArea == null) spawnArea = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!GameManager.GameStarted) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            if (currentEnemyCount < maxEnemies)
            {
                StartCoroutine(SpawnSequence());
            }
            timer = 0f;
        }
    }

    IEnumerator SpawnSequence()
    {
        if (enemyPrefabs.Length == 0 || spawnArea == null || indicatorPrefab == null) yield break;

        Vector2 randomPoint = GetRandomPointInBounds(spawnArea.bounds);
        Vector3 spawnPos = new Vector3(randomPoint.x, randomPoint.y, 0f);

        GameObject indicator = Instantiate(indicatorPrefab, spawnPos, Quaternion.identity);

        yield return new WaitForSeconds(warningDuration);

        Destroy(indicator);
        SpawnEnemy(spawnPos);
    }

    public void SpawnEnemy(Vector3 spawnPos)
    {
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject newEnemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        int currentWave = WaveManager.Instance != null ? WaveManager.Instance.currentWave : 1;

        EnemyHealth healthScript = newEnemy.GetComponent<EnemyHealth>();
        if (healthScript != null)
        {
            healthScript.SetSpawner(this);
            healthScript.ScaleStats(currentWave);
        }

        EnemyAttack attackScript = newEnemy.GetComponent<EnemyAttack>();
        if (attackScript != null) attackScript.ScaleDamage(currentWave);

        EnemyFollow followScript = newEnemy.GetComponent<EnemyFollow>();
        if (followScript != null) followScript.ScaleSpeed(currentWave);

        currentEnemyCount++;
    }

    Vector2 GetRandomPointInBounds(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }

    public void OnEnemyDestroyed()
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
    }
}