using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Components")]
    public SpriteRenderer gunSprite;

    public static bool isPanelOpen = false;

    void Update()
    {
        if (!GameManager.GameStarted) return;

        if (PauseManager.isPaused) return;

        if (isPanelOpen) return;

        RotateGun();
    }

    void RotateGun()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (gunSprite != null)
        {
            if (Mathf.Abs(angle) > 90)
            {
                gunSprite.flipY = true;
            }
            else
            {
                gunSprite.flipY = false;
            }
        }
    }
}