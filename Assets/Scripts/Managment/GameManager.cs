using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool GameStarted;
    [Header("Components")]
    public TextMeshProUGUI countdownText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        GameStarted = false;
        Time.timeScale = 0f;
    }

    public void StartButtonPressed()
    {
        StartCoroutine(CountdownRoutine());
        UIManager.Instance.ShowHUD(true);
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.SetWeaponHolder(true);
        }
        
    }

    IEnumerator CountdownRoutine()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.EnablePlayerVisualOnly();
        }

        countdownText.gameObject.SetActive(true);

        Time.timeScale = 1f;

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.gameObject.SetActive(false);

        //StartGame();
        WaveManager.Instance.StartNextWave();
    }

    void StartGame()
    {
        GameStarted = true;
    }

    public void EndGame()
    {
        GameStarted = false;

        Player player = FindObjectOfType<Player>();
        //if (player != null) player.StopMoving();

        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        foreach (EnemyHealth enemy in enemies)
        {
            enemy.GetComponent<EnemyFollow>().StopMoving();
        }

        Debug.Log("Oyun bitti, animasyonlar devam ediyor.");
    }
}
