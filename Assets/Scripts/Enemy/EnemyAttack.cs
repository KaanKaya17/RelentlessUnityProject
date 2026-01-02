using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public int damage = 10;
    public float attackCooldown = 1f;

    private bool canAttack = true;

    public bool CanAttack
    {
        get { return canAttack; }
    }

    public void ScaleDamage(int waveIndex)
    {
        damage += (waveIndex * 2);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!canAttack) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                StartCoroutine(AttackCooldown());
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
