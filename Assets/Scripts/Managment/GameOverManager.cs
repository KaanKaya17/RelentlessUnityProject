using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject contentContainer;
    public TextMeshProUGUI gameOverTitleText;

    [Header("Statistic UI")]
    public TextMeshProUGUI statsText;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        if (contentContainer != null) contentContainer.SetActive(false);
    }

    public void SetupGameOver()
    {
        Time.timeScale = 0f;

        gameOverPanel.SetActive(true);
        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        if (contentContainer != null) contentContainer.SetActive(false);

        if (gameOverTitleText != null)
        {
            gameOverTitleText.gameObject.SetActive(true);
            string fullText = "YOU DIED";
            gameOverTitleText.text = "";

            foreach (char letter in fullText)
            {
                gameOverTitleText.text += letter;
                yield return new WaitForSecondsRealtime(0.15f);
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }

        UpdateStatsDisplay();
        if (contentContainer != null) contentContainer.SetActive(true);
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

        statsText.text = "<b>Your Stats</b>\n" +
                         "<color=red>Damage:</color> " + damage + "\n" +
                         "<color=green>Speed:</color> " + speed.ToString("F1") + "\n" +
                         "<color=blue>Health:</color> " + currentHP + "/" + maxHP;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}