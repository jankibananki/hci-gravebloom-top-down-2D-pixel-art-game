using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float detectionRange = 8f;
    public float attackRange = 5f;

    [Header("Attack")]
    public GameObject projectilePrefab;
    public float projectileSpawnDistance = 0.6f;
    public float attackCooldown = 2f;
    public float castDelay = 0.35f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 moveDirection;
    private Vector2 lastDirection = Vector2.down;

    private bool isCasting = false;
    private bool attackOnCooldown = false;

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

        SetDirection(lastDirection);
    }

    void Update()
    {
        if (player == null)
            return;

        if (isCasting)
        {
            moveDirection = Vector2.zero;
            animator.SetBool("IsMoving", false);
            return;
        }

        float distance =
            Vector2.Distance(transform.position, player.position);

        // Player je predaleko
        if (distance > detectionRange)
        {
            moveDirection = Vector2.zero;
            animator.SetBool("IsMoving", false);
            return;
        }

        Vector2 directionToPlayer =
            ((Vector2)player.position -
             (Vector2)transform.position).normalized;

        // Player je u attack range-u
        if (distance <= attackRange)
        {
            moveDirection = Vector2.zero;

            animator.SetBool("IsMoving", false);

            FaceDirection(directionToPlayer);

            if (!attackOnCooldown)
                StartCoroutine(Cast(directionToPlayer));

            return;
        }

        // Player je primećen, ali nije dovoljno blizu
        moveDirection = directionToPlayer;

        animator.SetBool("IsMoving", true);

        FaceDirection(directionToPlayer);
    }

    void FixedUpdate()
    {
        if (isCasting)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity =
            moveDirection * moveSpeed;
    }

    void FaceDirection(Vector2 direction)
    {
        Vector2 cardinal;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            cardinal =
                new Vector2(Mathf.Sign(direction.x), 0);
        }
        else
        {
            cardinal =
                new Vector2(0, Mathf.Sign(direction.y));
        }

        lastDirection = cardinal;

        SetDirection(cardinal);
    }

    void SetDirection(Vector2 direction)
    {
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }

    IEnumerator Cast(Vector2 direction)
    {
        isCasting = true;
        attackOnCooldown = true;

        Vector2 cardinal;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            cardinal =
                new Vector2(Mathf.Sign(direction.x), 0);
        }
        else
        {
            cardinal =
                new Vector2(0, Mathf.Sign(direction.y));
        }

        animator.SetFloat("AimX", cardinal.x);
        animator.SetFloat("AimY", cardinal.y);

        animator.SetTrigger("Cast");

        // čeka da animacija dođe do dela gde baca spell
        yield return new WaitForSeconds(castDelay);

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
            enemyProjectile.Shoot(direction);

        isCasting = false;

        yield return new WaitForSeconds(attackCooldown);

        attackOnCooldown = false;
    }
}