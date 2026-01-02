using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;

    [Header("Components")]
    public TextMeshProUGUI healthText;
    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        healthText.text = currentHealth.ToString();

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        HitFlashPlayer flash = GetComponent<HitFlashPlayer>();
        if (flash != null)
        {
            flash.Flash();
        }

        if (CameraShake.Instance != null)
        {
            StartCoroutine(CameraShake.Instance.Shake(0.1f, 0.1f));
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            healthText.text = currentHealth.ToString();
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (healthBar != null) healthBar.SetHealth(currentHealth);
        healthText.text = currentHealth.ToString();

        HealFlash flash = GetComponent<HealFlash>();
        if (flash != null) {
            flash.Flash();
        }
    }

    public void Die()
    {
        WeaponController.isPanelOpen = true;
        if (isDead) return;
        isDead = true;

        StartCoroutine(DeathProcessRoutine());
    }

    IEnumerator DeathProcessRoutine()
    {
        isDead = true;
        Debug.Log("Oyuncu öldü, düþmanlar durduruluyor...");

        Player playerSettings = GetComponent<Player>();
        PlayerShoot playerShoot = GetComponent<PlayerShoot>();
        if (playerSettings != null) playerSettings.moveSpeed = 0f;
        if (playerShoot != null) playerShoot.enabled = false;

        EnemyFollow[] allEnemies = FindObjectsOfType<EnemyFollow>();
        foreach (EnemyFollow enemy in allEnemies)
        {
            enemy.SetDead();
        }

        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null) spawner.enabled = false;

        if (playerSettings != null) playerSettings.PlayDeathAnimation();

        yield return new WaitForSeconds(2f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame();
            int finalScore = ScoreManager.Instance != null ? ScoreManager.Instance.score : 0;
            int finalWave = WaveManager.Instance != null ? WaveManager.Instance.currentWave : 1;
            UIManager.Instance.ShowGameOver(finalScore, finalWave);
            FindObjectOfType<GameOverManager>().SetupGameOver();
        }
    }
}