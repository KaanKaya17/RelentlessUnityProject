using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Bullet Components")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Player Shot Settings")]
    public int currentDamage = 20;

    void Update()
    {
        if (!GameManager.GameStarted || PauseManager.isPaused) return;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint atanmadý! Lütfen PlayerShoot scriptine firePoint objesini sürükleyin.");
            return;
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 direction = (mouseWorldPos - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = currentDamage;
            bulletScript.SetDirection(direction);
        }
    }
}
