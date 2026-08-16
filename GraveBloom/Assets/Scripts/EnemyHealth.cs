using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 15;

    private int currentHealth;
    private bool isDead = false;

    private Animator animator;
    private EnemyAI enemyAI;
    private Rigidbody2D rb;
    private Collider2D enemyCollider;

    void Awake()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();

        Debug.Log("Enemy starting HP: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        Debug.Log(
            "Enemy took " + damage +
            " damage. HP: " + currentHealth
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("ENEMY DIED!");

        if (enemyAI != null)
            enemyAI.SetDead();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (enemyCollider != null)
            enemyCollider.enabled = false;

        if (animator != null)
        {
            animator.ResetTrigger("Cast");
            animator.SetBool("IsMoving", false);
            animator.SetTrigger("Die");
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}