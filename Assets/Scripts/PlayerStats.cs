using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public float damageMultiplier = 1f;
    public float flatDamageBonus = 0f;

    public float damageScalePerWave = 0.05f; // %5 artýþ

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ScalePlayerPower(int waveIndex)
    {
        // Oyuncuya her dalga baþýnda küçük bir pasif güç ver
        // Bu, düþmanlarýn devasa can artýþýna karþý oyuncunun tamamen ezilmesini engeller
        damageMultiplier += damageScalePerWave;
    }
}