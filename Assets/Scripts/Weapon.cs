using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Components")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Visual")]
    public Transform visual; //  YENÝ
    private SpriteRenderer sr;

    [Header("Weapon Stats")]
    public float fireRate = 0.5f;
    public int damage = 20;

    [Header("Recoil")]
    public float recoilDistance = 0.16f;
    public float recoverySpeed = 20f;

    float nextFireTime;
    Vector3 visualOriginalLocalPos;

    void Start()
    {
        sr = visual.GetComponent<SpriteRenderer>();
        visualOriginalLocalPos = visual.localPosition;
    }

    void Update()
    {
        if (!GameManager.GameStarted || PauseManager.isPaused || WeaponController.isPanelOpen)
            return;

        RotateTowardsMouse();

        //  recoil recovery (yuvarlak BOZULMAZ)
        visual.localPosition = Vector3.Lerp(
            visual.localPosition,
            visualOriginalLocalPos,
            Time.deltaTime * recoverySpeed
        );

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

        sr.flipY = Mathf.Abs(angle) > 90;
    }

    void Shoot()
    {
        if (!firePoint || !bulletPrefab) return;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            transform.rotation
        );

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            float finalDamage =
                (damage + PlayerStats.Instance.flatDamageBonus) *
                PlayerStats.Instance.damageMultiplier;

            bulletScript.damage = Mathf.RoundToInt(finalDamage);
            bulletScript.SetDirection(transform.right);
        }

        //  GERÇEK BROTATO RECOIL (SADECE GÖRSEL)
        visual.localPosition = visualOriginalLocalPos + Vector3.left * recoilDistance;
    }
}
