using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int money = 0;
    [Header("Components")]
    public static MoneyManager Instance;
    public TextMeshProUGUI moneyText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateMoneyUI();
    }

    public void addMoney(int amount)
    {
        money += amount;
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyUI();
            return true;
        }
        return false;
    }

    public void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: " + money;
        }
    }
}
