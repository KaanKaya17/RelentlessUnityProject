using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    public GameObject hudPanel;   
    public GameObject shopPanel;  
    public GameObject gameOverPanel;

    [Header("Texts")]
    public TextMeshProUGUI waveTimerText;
    public TextMeshProUGUI waveCountText;

    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalWaveText;
    //public TextMeshProUGUI scoreText;

    [Header("Health Bar")]
    public Slider healthSlider;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowHUD(bool state)
    {
        hudPanel.SetActive(state);
    }

    public void ShowGameOver(int score, int wave)
    {
        hudPanel.SetActive(false); 
        gameOverPanel.SetActive(true); 
        finalScoreText.text = "Score : " + score;
        finalWaveText.text = "Wave : " + wave;
        Time.timeScale = 0f;
    }
}