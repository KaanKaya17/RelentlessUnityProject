using UnityEngine;

public class HealthPickupSpawner : MonoBehaviour
{
    [Header("Health Pickup Spawner Settings")]
    public GameObject healthPickupPrefab;
    public BoxCollider2D spawnArea;

    [Header("Pickup Settings")]
    public float spawnInterval = 10f;
    public int maxPickupsOnMap = 5;

    private float timer;

    void Update()
    {
        if (GameManager.GameStarted == false) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            int currentPickups = GameObject.FindGameObjectsWithTag("HealthPickup").Length;

            if (currentPickups < maxPickupsOnMap)
            {
                SpawnPickup();
            }

            timer = 0f;
        }
    }

    void SpawnPickup()
    {
        if (healthPickupPrefab == null || spawnArea == null) return;

        Vector2 pos = GetRandomPointInBounds(spawnArea.bounds);

        Instantiate(healthPickupPrefab, pos, Quaternion.identity);
    }

    Vector2 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }
}