using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 50;
    public int scoreValue = 10;

    private EnemySpawner mySpawner;
    private Animator anim;
    private bool isDead = false;
    private int currentHealth;

    [Header("Experience Settings")]
    public int experienceValue = 10;

    [Header("Effects")]
    public GameObject damagePopupPrefab;
    //public Transform popupSpawnPoint;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>(); 
    }

    public void SetSpawner(EnemySpawner spawner)
    {
        mySpawner = spawner;
    }

    public void ScaleStats(int waveIndex)
    {
        maxHealth += (waveIndex * 15);
        currentHealth = maxHealth;

        experienceValue += (waveIndex * 2);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        HitFlash flasher = GetComponent<HitFlash>();
        if (flasher != null)
        {
            flasher.Flash();
        }

        if (damagePopupPrefab != null)
        {
            GameObject popup = Instantiate(damagePopupPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(damage);
        }


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (ScoreManager.Instance != null) ScoreManager.Instance.AddScore(scoreValue);
        if (mySpawner != null) mySpawner.OnEnemyDestroyed();

        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.addMoney(15);
            Debug.Log("Money: " + MoneyManager.Instance.money);
        }

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.AddXP(experienceValue);
        }

        EnemyFollow followScript = GetComponent<EnemyFollow>();
        if (followScript != null)
        {
            followScript.SetDead();
        }

        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = false;
        }

        if (anim != null) anim.SetTrigger("Die");

        Destroy(gameObject, 1.5f);
    }
}