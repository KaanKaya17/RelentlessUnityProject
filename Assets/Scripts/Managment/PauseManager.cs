using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;
    [Header("Components")]
    public GameObject pausePanel;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip openSound;     
    public AudioClip closeSound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (UIManager.Instance != null) UIManager.Instance.ShowHUD(true);

        if (audioSource != null && closeSound != null)
            audioSource.PlayOneShot(closeSound);
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan çýkýlýyor...");
        Application.Quit();
    }

    void Pause()
    {
        if (ShopManager.Instance != null && ShopManager.Instance.shopPanel.activeSelf) return;

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        if (audioSource != null && openSound != null)
            audioSource.PlayOneShot(openSound);

        if (UIManager.Instance != null) UIManager.Instance.ShowHUD(false);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}