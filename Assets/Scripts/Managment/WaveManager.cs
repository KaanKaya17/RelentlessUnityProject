using UnityEngine;
using TMPro;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("UI Components")]
    public TextMeshProUGUI waveTimerText;
    public TextMeshProUGUI currentWaveText;

    [Header("Wave Settings")]
    public float waveDuration = 30f;
    public int currentWave = 1;
    private float currentTimer;
    private bool isWaveActive = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void StartNextWave()
    {
        if (currentWaveText != null)
            currentWaveText.text = "Wave: " + currentWave;

        currentTimer = waveDuration;
        isWaveActive = true;

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ScalePlayerPower(currentWave);
        }

        UpdateSpawnerDifficulty();
        GameManager.GameStarted = true;
    }

    void Update()
    {
        if (!isWaveActive || !GameManager.GameStarted) return;

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null && playerHealth.isDead)
        {
            if (waveTimerText != null) waveTimerText.text = "";
            return;
        }

        currentTimer -= Time.deltaTime;

        if (currentTimer < waveDuration / 2)
        {
            EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
            if (spawner != null) spawner.spawnInterval = 0.3f;
        }

        if (waveTimerText != null)
            waveTimerText.text = Mathf.Ceil(currentTimer).ToString();

        if (currentTimer <= 0)
        {
            EndWave();
        }
    }

    void EndWave()
    {
        isWaveActive = false;
        GameManager.GameStarted = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) Destroy(enemy);

        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null) spawner.currentEnemyCount = 0;

        currentWave++;

        GameManager.Instance.StartButtonPressed();

        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.OpenShop();
        }
        else
        {
            StartNextWave();
        }
    }

    void UpdateSpawnerDifficulty()
    {
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.maxEnemies = 10 + (currentWave * 5);
            spawner.spawnInterval = Mathf.Max(0.5f, 2f - (currentWave * 0.2f));
        }
    }
}