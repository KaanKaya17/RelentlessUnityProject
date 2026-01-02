using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic; 

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [System.Serializable]
    public class WeaponData
    {
        public string weaponName;   
        public GameObject prefab;   
        public int price;           
    }

    [Header("Weapon Progression (Sýralý Silahlar)")]
    public List<WeaponData> weaponSequence; 
    private int currentWeaponIndex = 0;    

    [Header("Weapon UI Components")]
    public TextMeshProUGUI weaponNameText;  
    public TextMeshProUGUI weaponPriceText; 
    public GameObject buyWeaponButton;      


    [Header("Management")]
    public GameObject shopPanel;

    [Header("Sýralý Açýlýþ Ayarlarý")]
    public GameObject waveFinishedText;
    public GameObject upgradeButtonsContainer;

    [Header("Damage Upgrade Ayarlarý")]
    public int damagePrice = 50;
    public int speedPrice = 75;
    public float priceMultiplier = 1.5f;
    public TextMeshProUGUI damagePriceText;
    public TextMeshProUGUI speedPriceText;
    public TextMeshProUGUI maxHealthPriceText;

    [Header("Health Upgrade Ayarlarý")]
    public int healthPrice = 100;

    [Header("Ýstatistik UI")]
    public TextMeshProUGUI statsText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        UpdateStatsDisplay();

        if (waveFinishedText != null) waveFinishedText.SetActive(false);
        if (upgradeButtonsContainer != null) upgradeButtonsContainer.SetActive(false);

        UpdateShopUI();
    }

    public void BuyNewWeapon()
    {
        if (currentWeaponIndex >= weaponSequence.Count) return;

        WeaponData nextWeapon = weaponSequence[currentWeaponIndex];

        if (MoneyManager.Instance != null && MoneyManager.Instance.SpendMoney(nextWeapon.price))
        {
            WeaponInventory inventory = FindObjectOfType<WeaponInventory>();
            if (inventory != null)
            {
                inventory.AddWeapon(nextWeapon.prefab);

                currentWeaponIndex++;

                UpdateShopUI();
                UpdateStatsDisplay();
            }
        }
    }
    // ----------------------------------------

    public void OpenShop()
    {
        StartCoroutine(OpenShopSequence());
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            // player.SetWeaponHolder(false); 
        }
    }

    IEnumerator OpenShopSequence()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null && playerHealth.isDead) yield break;

        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.ForceIdle();
            player.getPlayerToCenter();
        }

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.ResetShake();
        }

        Time.timeScale = 0f;

        shopPanel.SetActive(true);
        upgradeButtonsContainer.SetActive(false);
        if (UIManager.Instance != null) UIManager.Instance.ShowHUD(false);

        if (waveFinishedText != null)
        {
            waveFinishedText.SetActive(true);
            TextMeshProUGUI textComp = waveFinishedText.GetComponent<TextMeshProUGUI>();
            string tamMetin = "WAVE OVER";
            textComp.text = "";

            foreach (char harf in tamMetin)
            {
                textComp.text += harf;
                yield return new WaitForSecondsRealtime(0.1f);
            }
            yield return new WaitForSecondsRealtime(0.8f);
            waveFinishedText.SetActive(false);
        }

        // 3. ADIM: Market içeriðini göster
        upgradeButtonsContainer.SetActive(true);
        UpdateShopUI();
        UpdateStatsDisplay();

        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.UpdateMoneyUI();
        }
    }

    public void UpdateShopUI()
    {
        if (damagePriceText != null) damagePriceText.text = "Fiyat: " + damagePrice;
        if (speedPriceText != null) speedPriceText.text = "Fiyat: " + speedPrice;
        if (maxHealthPriceText != null) maxHealthPriceText.text = "Fiyat: " + healthPrice;

        if (currentWeaponIndex < weaponSequence.Count)
        {
            WeaponData nextWeapon = weaponSequence[currentWeaponIndex];

            if (weaponNameText != null) weaponNameText.text = nextWeapon.weaponName; // Örn: "Buy AK-47"
            if (weaponPriceText != null) weaponPriceText.text = "Fiyat: " + nextWeapon.price;

            if (buyWeaponButton != null) buyWeaponButton.SetActive(true);
        }
        else
        {
            if (weaponNameText != null) weaponNameText.text = "SOLD OUT";
            if (weaponPriceText != null) weaponPriceText.text = "---";

            if (buyWeaponButton != null) buyWeaponButton.SetActive(false);
        }
    }

    public void UpgradeDamage()
    {
        if (MoneyManager.Instance != null && MoneyManager.Instance.SpendMoney(damagePrice))
        {
            PlayerShoot player = FindObjectOfType<PlayerShoot>();
            if (player != null) player.currentDamage += 5;
            if (PlayerStats.Instance != null) PlayerStats.Instance.flatDamageBonus += 5f;


            damagePrice = Mathf.RoundToInt(damagePrice * priceMultiplier);
            UpdateStatsDisplay();
            UpdateShopUI();
        }
    }

    public void UpgradeSpeed()
    {
        if (MoneyManager.Instance != null && MoneyManager.Instance.SpendMoney(speedPrice))
        {
            Player playerSettings = FindObjectOfType<Player>();
            if (playerSettings != null) playerSettings.moveSpeed += 0.2f;
            speedPrice = Mathf.RoundToInt(speedPrice * priceMultiplier);
            UpdateShopUI();
            UpdateStatsDisplay();
        }
    }

    public void UpgradeMaxHealth()
    {
        if (MoneyManager.Instance != null && MoneyManager.Instance.SpendMoney(healthPrice))
        {
            PlayerHealth health = FindObjectOfType<PlayerHealth>();
            if (health != null) health.Heal(5);
            healthPrice = Mathf.RoundToInt(healthPrice * priceMultiplier);
            UpdateStatsDisplay();
            UpdateShopUI();
        }
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

        statsText.text = "<b>Current Stats</b>\n" +
                         "<color=red>Damage:</color> " + damage + "\n" +
                         "<color=green>Speed:</color> " + speed.ToString("F1") + "\n" +
                         "<color=blue>Health:</color> " + currentHP + "/" + maxHP;
    }

    public void ClearAllBullets()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
        Debug.Log("Ekrandaki tüm mermiler temizlendi!");
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        ClearAllBullets();

        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartButtonPressed();
            if (UIManager.Instance != null) UIManager.Instance.ShowHUD(true);
            Player player = FindObjectOfType<Player>();
            // if (player != null) player.SetWeaponHolder(true);
        }
    }
}