using UnityEngine;
using System.Collections.Generic;

public class WeaponInventory : MonoBehaviour
{
    [Header("Weapon Startup")]
    public GameObject defaultWeaponPrefab;
    public List<GameObject> weaponPrefabs;
    public Transform weaponHolder;
    public float radius = 1.5f;

    private List<GameObject> activeWeapons = new List<GameObject>();

    void Start()
    {
        if (defaultWeaponPrefab != null)
        {
            AddWeapon(defaultWeaponPrefab);
        }
    }

    public void AddWeapon(GameObject weaponPrefab)
    {
        GameObject newWeapon = Instantiate(weaponPrefab, weaponHolder);
        activeWeapons.Add(newWeapon);
        ArrangeWeapons();
    }

    void ArrangeWeapons()
    {
        int count = activeWeapons.Count;
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            activeWeapons[i].transform.localPosition = newPos;
        }
    }
}