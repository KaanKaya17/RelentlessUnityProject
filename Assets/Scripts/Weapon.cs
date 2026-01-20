using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Components")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    [Header("Weapon Stats")]
    public float fireRate = 0.5f;
    public int damage = 20;

    private float nextFireTime;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!GameManager.GameStarted || PauseManager.isPaused || WeaponController.isPanelOpen) return;

        RotateTowardsMouse();

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Mathf.Abs(angle) > 90)
            sr.flipY = true;
        else
            sr.flipY = false;
    }

    void Shoot()
    {
        if (firePoint == null || bulletPrefab == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            float finalDamage = (this.damage + PlayerStats.Instance.flatDamageBonus) * PlayerStats.Instance.damageMultiplier;

            bulletScript.damage = Mathf.RoundToInt(finalDamage);
            bulletScript.SetDirection(transform.right);
        }
    }
}