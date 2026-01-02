using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Base Stats")]
    public int maxHealth = 50;
    public int scoreValue = 10;
    public int experienceValue = 10;
    public int moneyValue = 10;

    [Header("Scaling (Dalga Baþýna Artýþ)")]
    public int healthGainPerWave = 15;
    public int xpGainPerWave = 2;

    private int currentHealth;
    private EnemySpawner mySpawner;
    private Animator anim;
    private bool isDead = false;

    [Header("Effects")]
    public GameObject damagePopupPrefab;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetSpawner(EnemySpawner spawner)
    {
        mySpawner = spawner;
    }

    public void ScaleStats(int waveIndex)
    {
        // 1. dalgada artýþ olmamasý için waveIndex - 1
        int multiplier = Mathf.Max(0, waveIndex - 1);

        maxHealth += (multiplier * healthGainPerWave);
        currentHealth = maxHealth;
        experienceValue += (multiplier * xpGainPerWave);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        // Hit Flash Efekti
        if (TryGetComponent<HitFlash>(out HitFlash flasher)) flasher.Flash();

        // Damage Popup
        if (damagePopupPrefab != null)
        {
            GameObject popup = Instantiate(damagePopupPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(damage);
        }

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // Ödülleri Daðýt
        if (ScoreManager.Instance != null) ScoreManager.Instance.AddScore(scoreValue);
        if (MoneyManager.Instance != null) MoneyManager.Instance.addMoney(moneyValue);
        if (LevelManager.Instance != null) LevelManager.Instance.AddXP(experienceValue);

        if (mySpawner != null) mySpawner.OnEnemyDestroyed();

        // Hareketini durdur
        if (TryGetComponent<EnemyFollow>(out EnemyFollow follow)) follow.SetDead();

        // Çarpýþmayý kapat
        if (TryGetComponent<Collider2D>(out Collider2D col)) col.enabled = false;

        if (anim != null) anim.SetTrigger("Die");

        Destroy(gameObject, 1.5f);
    }
}