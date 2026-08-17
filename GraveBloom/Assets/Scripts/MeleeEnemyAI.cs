using System.Collections;
using UnityEngine;

public class MeleeEnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float detectionRange = 7f;
    public float attackRange = 1.2f;

    [Header("Attack")]
    public int damage = 1;
    public float attackCooldown = 1.5f;

    // Koliko posle početka animacije stvarno udara
    public float attackHitDelay = 0.35f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 moveDirection;

    private bool isAttacking = false;
    private bool attackOnCooldown = false;
    private bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
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

        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        // // Predaleko je player
        // if (distance > detectionRange)
        // {
        //     StopMoving();
        //     return;
        // }

        Vector2 directionToPlayer =
            ((Vector2)player.position -
             (Vector2)transform.position).normalized;

        // Dovoljno blizu za udarac
        if (distance <= attackRange)
        {
            StopMoving();

            FaceDirection(directionToPlayer);

            if (!attackOnCooldown)
                StartCoroutine(Attack());

            return;
        }

        // Juri playera
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

    void StopMoving()
    {
        moveDirection = Vector2.zero;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.SetBool("IsMoving", false);
    }

    void FaceDirection(Vector2 direction)
    {
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

        animator.SetFloat(
            "MoveX",
            cardinal.x
        );

        animator.SetFloat(
            "MoveY",
            cardinal.y
        );
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        attackOnCooldown = true;

        StopMoving();

        animator.SetTrigger("Attack");

        // čekamo da mač dođe do dela animacije
        // gde stvarno udara
        yield return new WaitForSeconds(
            attackHitDelay
        );

        if (isDead || player == null)
            yield break;

        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        // proveri da li je player i dalje blizu
        if (distance <= attackRange + 0.25f)
        {
            PlayerHealth health =
                player.GetComponent<PlayerHealth>();

            if (health != null)
                health.TakeDamage(damage);
        }

        isAttacking = false;

        yield return new WaitForSeconds(
            attackCooldown
        );

        attackOnCooldown = false;
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

    public bool IsDead()
    {
        return isDead;
    }
}