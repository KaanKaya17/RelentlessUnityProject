using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Heal Settings")]
    public int healAmount = 20;
    [Header("Components")]
    public GameObject healEffectPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerHealth>(out PlayerHealth health))
            {
                health.Heal(healAmount);
            }

            if (healEffectPrefab != null)
            {
                Vector3 spawnPos = other.transform.position + Vector3.up * 1f;
                spawnPos.z = -1f;
                Instantiate(healEffectPrefab, spawnPos, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}