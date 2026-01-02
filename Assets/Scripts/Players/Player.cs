using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;

    [Header("Components")]
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    private SpriteRenderer sr;

    [Header("Sound Settings")]
    public AudioSource footstepAudioSource;
    public AudioClip footstepClip;
    public float footstepRate = 0.5f;
    private float footstepTimer;

    [Header("Weapon Settings")]
    public GameObject weaponHolder;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        getPlayerToCenter();

        if (!GameManager.GameStarted)
        {
            sr.enabled = false;
            animator.enabled = false;
            if (weaponHolder != null) weaponHolder.SetActive(false);

        }
    }

    void Update()
    {
        if (!GameManager.GameStarted) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bool isMoving = movement != Vector2.zero;
        animator.SetBool("isMoving", isMoving);
        HandleFootsteps();

        if (movement.x > 0)
        {
            sr.flipX = false;
        }
        else if (movement.x < 0)
        {
            sr.flipX = true; 
        }
    }

    void HandleFootsteps()
    {
        bool isMoving = movement.magnitude > 0.5f;

        if (isMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                PlayFootstepSound();
                footstepTimer = footstepRate;
            }
        }
    }

    void PlayFootstepSound()
    {
        if (footstepAudioSource != null && footstepClip != null)
        {
            footstepAudioSource.pitch = Random.Range(0.8f, 1.2f);
            footstepAudioSource.PlayOneShot(footstepClip);
        }
    }

    public void EnablePlayerVisualOnly()
    {
        sr.enabled = true;
        animator.enabled = true;
    }

    public void SetWeaponHolder(bool state)
    {
        if (weaponHolder != null)
        {
            weaponHolder.SetActive(state);
        }
        else
        {
            Debug.LogError("WeaponHolder atanmamış! Lütfen Inspector'dan sürükle.");
        }
    }

    public void EnablePlayer()
    {
        sr.enabled = true;
        animator.enabled = true;
    }

    public void PlayDeathAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }

    public void ForceIdle()
    {
        movement = Vector2.zero;
        if (rb != null) rb.linearVelocity = Vector2.zero;
        if (animator != null) animator.SetBool("isMoving", false);
    }

    public void getPlayerToCenter()
    {
        transform.position = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (!GameManager.GameStarted)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = movement.normalized * moveSpeed;
    }
}