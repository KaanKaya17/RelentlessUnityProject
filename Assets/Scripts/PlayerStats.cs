using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public float damageMultiplier = 1f;
    public float flatDamageBonus = 0f; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
}