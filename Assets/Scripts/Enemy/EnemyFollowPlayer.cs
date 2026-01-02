using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Movemenent")]
    public float moveSpeed = 3f;
    public float speedGainPerWave = 0.2f; // Her dalga ne kadar hýzlanacak?
    private bool isDead = false;

    [Header("Components")]
    private Transform player;
    private Rigidbody2D rb;
    private EnemyAttack enemyAttack;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    public void ScaleSpeed(int waveIndex)
    {
        moveSpeed += (Mathf.Max(0, waveIndex - 1) * speedGainPerWave);
    }

    public void SetDead()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
    }

    public void StopMoving()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetFloat("Speed", 0);
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (!GameManager.GameStarted)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isMoving", false);
            return;
        }

        if (player == null) return;

        if (enemyAttack != null && !enemyAttack.CanAttack)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isMoving", false);
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;

        animator.SetBool("isMoving", rb.linearVelocity.magnitude > 0.1f);

        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

}
