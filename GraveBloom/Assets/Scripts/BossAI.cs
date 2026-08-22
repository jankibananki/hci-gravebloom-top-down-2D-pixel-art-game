using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.6f;
    public float attackRange = 1.8f;

    [Header("Attack")]
    public int damage = 2;
    public float attackCooldown = 1.7f;
    public float attackHitDelay = 0.55f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 moveDirection;
    private Vector2 lastDirection = Vector2.down;

    private bool isAttacking = false;
    private bool attackOnCooldown = false;
    private bool isDead = false;
    private EnemySFX enemySFX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemySFX = GetComponent<EnemySFX>();
    }

    void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        SetDirection(lastDirection);
    }

    void Update()
    {
        if (isDead || player == null)
        {
            StopMoving();
            return;
        }

        if (isAttacking)
        {
            StopMoving();
            return;
        }

        Vector2 directionToPlayer =
            ((Vector2)player.position -
             (Vector2)transform.position).normalized;

        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        // Dovoljno blizu za attack
        if (distance <= attackRange)
        {
            StopMoving();

            FaceDirection(directionToPlayer);

            if (!attackOnCooldown)
                StartCoroutine(Attack());

            return;
        }

        // inace odmah juri playera
        moveDirection = directionToPlayer;

        animator.SetBool("IsMoving", true);

        FaceDirection(directionToPlayer);
    }

    void FixedUpdate()
    {
        if (isDead || isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity =
            moveDirection * moveSpeed;
    }

    void FaceDirection(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return;

        Vector2 cardinal;

        if (Mathf.Abs(direction.x) >
            Mathf.Abs(direction.y))
        {
            cardinal =
                new Vector2(
                    Mathf.Sign(direction.x),
                    0
                );
        }
        else
        {
            cardinal =
                new Vector2(
                    0,
                    Mathf.Sign(direction.y)
                );
        }

        lastDirection = cardinal;

        SetDirection(cardinal);
    }

    void SetDirection(Vector2 direction)
    {
        animator.SetFloat(
            "MoveX",
            direction.x
        );

        animator.SetFloat(
            "MoveY",
            direction.y
        );
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        attackOnCooldown = true;

        StopMoving();

        // ponovo se okrene ka playeru
        // neposredno pre napada
        if (player != null)
        {
            Vector2 direction =
                ((Vector2)player.position -
                 (Vector2)transform.position).normalized;

            FaceDirection(direction);
        }

        animator.SetTrigger("Attack");

        if (enemySFX != null)
            enemySFX.PlayAttack();  

        // pusti attack animaciju da zavrsi
        yield return new WaitForSeconds(0.25f);

        isAttacking = false;

        yield return new WaitForSeconds(
            attackCooldown
        );

        attackOnCooldown = false;
    }

    void StopMoving()
    {
        moveDirection = Vector2.zero;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.SetBool("IsMoving", false);
    }

    public void SetDead()
    {
        if (isDead)
            return;

        isDead = true;

        StopAllCoroutines();

        StopMoving();

        animator.ResetTrigger("Attack");
    }

    public bool IsBossDead()
    {
        return isDead;
    }

    public void DealAttackDamage()
    {
        if (isDead || player == null)
            return;

        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        if (distance <= attackRange + 0.35f)
        {
            PlayerHealth health =
                player.GetComponent<PlayerHealth>();

            if (health != null)
                health.TakeDamage(damage);
        }
    }
}
