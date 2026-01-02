using UnityEngine;

public class CameraBobbing2D : MonoBehaviour
{
    [Header("Yürüyüş Ayarları")]
    public float bobbingSpeed = 12f;
    public float bobbingAmount = 0.1f;

    [Header("Referanslar")]
    public Rigidbody2D playerRb;

    private float timer = 0;
    private Vector3 initialLocalPos;
    private CameraShake shakeScript;

    void Start()
    {
        initialLocalPos = transform.localPosition;
        shakeScript = GetComponent<CameraShake>();

        if (playerRb == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerRb = player.GetComponent<Rigidbody2D>();
        }
    }

    void LateUpdate() // Update yerine LateUpdate kullanarak titremeyi önleyebiliriz
    {
        if (playerRb == null) return;

        float speed = playerRb.linearVelocity.magnitude;
        Vector3 bobbingOffset = Vector3.zero;

        if (speed > 0.1f)
        {
            timer += Time.deltaTime * bobbingSpeed;
            // Sadece Y ekseninde küçük bir sapma hesapla
            bobbingOffset.y = Mathf.Sin(timer) * bobbingAmount;
        }
        else
        {
            timer = 0;
        }

        // initialLocalPos üzerine sadece yürüme efektini ekle
        // CameraShake kendi içinde transform.localPosition'ı değiştirdiği için 
        // buradaki atama Shake bittiği anda devreye girer.
        transform.localPosition = initialLocalPos + bobbingOffset;
    }
}