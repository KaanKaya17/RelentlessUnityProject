using TMPro;
using UnityEngine;
using System.Collections;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager Instance;
    public GameObject levelUpPanel;

    [Header("Open Scene Components")]
    public GameObject levelUpTitleTextObj;
    public GameObject upgradeContent;

    [Header("Statistic UI")]
    public TextMeshProUGUI statsText;

    [Header("Volume Settings")]
    public AudioSource audioSource;
    public AudioClip levelUpSound;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        UpdateStatsDisplay();
        if (levelUpTitleTextObj != null) levelUpTitleTextObj.SetActive(false);
        if (upgradeContent != null) upgradeContent.SetActive(false);
    }

    public void OpenLevelUpMenu()
    {
        if (audioSource != null && levelUpSound != null)
        {
            audioSource.PlayOneShot(levelUpSound);
        }

        Time.timeScale = 0f;
        StartCoroutine(LevelUpSequence());
        WeaponController.isPanelOpen = true;
    }

    IEnumerator LevelUpSequence()
    {
        levelUpPanel.SetActive(true);
        upgradeContent.SetActive(false);
        if (UIManager.Instance != null) UIManager.Instance.ShowHUD(false);

        if (levelUpTitleTextObj != null)
        {
            levelUpTitleTextObj.SetActive(true);
            TextMeshProUGUI titleText = levelUpTitleTextObj.GetComponent<TextMeshProUGUI>();
            string fullText = "LEVEL UP!";
            titleText.text = "";

            foreach (char letter in fullText)
            {
                titleText.text += letter;
                yield return new WaitForSecondsRealtime(0.07f);
            }

            yield return new WaitForSecondsRealtime(0.25f);
        }

        upgradeContent.SetActive(true);
        UpdateStatsDisplay();
    }
    public void UpgradeDamage()
    {
        PlayerShoot player = FindObjectOfType<PlayerShoot>();
        if (player != null) player.currentDamage += 5;
        if (PlayerStats.Instance != null) PlayerStats.Instance.flatDamageBonus += 5f;
        UpdateStatsDisplay();
        CloseMenu();
    }

    public void UpgradeSpeed()
    {
        Player playerMove = FindObjectOfType<Player>();
        if (playerMove != null) playerMove.moveSpeed += 0.3f;
        UpdateStatsDisplay();
        CloseMenu();
    }

    public void UpgradeHealth()
    {
        PlayerHealth health = FindObjectOfType<PlayerHealth>();
        if (health != null) health.Heal(15);
        UpdateStatsDisplay();
        CloseMenu();
    }

    public void UpdateStatsDisplay()
    {
        if (statsText == null) return;
        Player player = FindObjectOfType<Player>();
        PlayerShoot playerShoot = FindObjectOfType<PlayerShoot>();
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

        float damage = playerShoot != null ? playerShoot.currentDamage : 0;
        float speed = player != null ? player.moveSpeed : 0;
        int currentHP = playerHealth != null ? playerHealth.currentHealth : 0;
        int maxHP = playerHealth != null ? playerHealth.maxHealth : 0;

        statsText.text = "<b>GÜNCEL DURUM</b>\n" +
                         "<color=red>Hasar:</color> " + damage + "\n" +
                         "<color=green>Hýz:</color> " + speed.ToString("F1") + "\n" +
                         "<color=blue>Can:</color> " + currentHP + "/" + maxHP;
    }

    void CloseMenu()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
        if (UIManager.Instance != null) UIManager.Instance.ShowHUD(true);

        if (levelUpTitleTextObj != null) levelUpTitleTextObj.SetActive(false);
        WeaponController.isPanelOpen = false;
    }
}