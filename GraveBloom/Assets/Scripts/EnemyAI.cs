using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float detectionRange = 8f;
    public float attackRange = 5f;

    [Header("Obstacle Avoidance")]
    public LayerMask obstacleLayer;
    public float obstacleCheckDistance = 1.2f;
    public float obstacleCheckRadius = 0.3f;

    [Header("Attack")]
    public GameObject projectilePrefab;
    public float projectileSpawnDistance = 0.6f;
    public float attackCooldown = 2f;
    public float castDelay = 0.35f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private EnemySFX enemySFX;

    private Vector2 moveDirection;
    private Vector2 lastDirection = Vector2.down;

    private bool isCasting = false;
    private bool attackOnCooldown = false;
    private bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // zvuk skeletona
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
        if (isDead)
        {
            moveDirection = Vector2.zero;
            return;
        }

        if (player == null)
            return;

        if (isCasting)
        {
            moveDirection = Vector2.zero;
            animator.SetBool("IsMoving", false);
            return;
        }

        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        Vector2 directionToPlayer =
            ((Vector2)player.position -
             (Vector2)transform.position).normalized;

        // player je u attack range-u
        if (distance <= attackRange)
        {
            moveDirection = Vector2.zero;

            animator.SetBool(
                "IsMoving",
                false
            );

            FaceDirection(directionToPlayer);

            if (!attackOnCooldown)
                StartCoroutine(
                    Cast(directionToPlayer)
                );

            return;
        }

        // idi prema playeru i izbegavaj prepreke
        moveDirection =
            GetObstacleAvoidanceDirection(
                directionToPlayer
            );

        animator.SetBool(
            "IsMoving",
            moveDirection != Vector2.zero
        );

        FaceDirection(moveDirection);
    }

    void FixedUpdate()
    {
        if (isDead || isCasting)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity =
            moveDirection * moveSpeed;
    }

    Vector2 GetObstacleAvoidanceDirection(
        Vector2 desiredDirection
    )
    {
        RaycastHit2D frontHit =
            Physics2D.CircleCast(
                transform.position,
                obstacleCheckRadius,
                desiredDirection,
                obstacleCheckDistance,
                obstacleLayer
            );

        // nema prepreke
        if (!frontHit)
            return desiredDirection;

        Vector2 leftDirection =
            new Vector2(
                -desiredDirection.y,
                desiredDirection.x
            ).normalized;

        Vector2 rightDirection =
            new Vector2(
                desiredDirection.y,
                -desiredDirection.x
            ).normalized;

        float leftClearance =
            GetClearance(leftDirection);

        float rightClearance =
            GetClearance(rightDirection);

        Vector2 avoidanceDirection;

        if (leftClearance > rightClearance)
            avoidanceDirection = leftDirection;
        else
            avoidanceDirection = rightDirection;

        Vector2 combinedDirection =
            desiredDirection * 0.35f +
            avoidanceDirection * 0.65f;

        return combinedDirection.normalized;
    }

    float GetClearance(Vector2 direction)
    {
        RaycastHit2D hit =
            Physics2D.CircleCast(
                transform.position,
                obstacleCheckRadius,
                direction,
                obstacleCheckDistance,
                obstacleLayer
            );

        if (!hit)
            return obstacleCheckDistance;

        return hit.distance;
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

    IEnumerator Cast(Vector2 direction)
    {
        isCasting = true;
        attackOnCooldown = true;

        rb.linearVelocity =
            Vector2.zero;

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
            "AimX",
            cardinal.x
        );

        animator.SetFloat(
            "AimY",
            cardinal.y
        );

        animator.SetTrigger("Cast");

        yield return new WaitForSeconds(
            castDelay
        );

        // ako je umro dok castuje
        if (isDead)
            yield break;

        Vector2 spawnPosition =
            (Vector2)transform.position +
            direction * projectileSpawnDistance;

        GameObject projectile =
            Instantiate(
                projectilePrefab,
                spawnPosition,
                Quaternion.identity
            );

        EnemyProjectile enemyProjectile =
            projectile.GetComponent<EnemyProjectile>();

        if (enemyProjectile != null)
        {
            enemyProjectile.Shoot(direction);
        }

        // ATTACK SOUND
        if (enemySFX != null)
        {
            enemySFX.PlayAttack();
        }

        isCasting = false;

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

        moveDirection =
            Vector2.zero;

        rb.linearVelocity =
            Vector2.zero;

        animator.SetBool(
            "IsMoving",
            false
        );

        animator.ResetTrigger("Cast");
    }
}
