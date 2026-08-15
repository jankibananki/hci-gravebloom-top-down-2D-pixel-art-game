using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float spawnDistance = 0.6f;

    private PlayerAim aim;
    private Animator animator;

    void Awake()
    {
        aim = GetComponent<PlayerAim>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Mouse.current == null)
            return;

        // LEVI KLIK = normal spell
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cast();
        }
    }

    void Cast()
    {
        Vector2 direction = aim.AimDirection;

        // Biramo najbližu od 4 postojeće cast animacije
        Vector2 cardinalDirection = aim.GetCardinalDirection();

        animator.SetFloat("AimX", cardinalDirection.x);
        animator.SetFloat("AimY", cardinalDirection.y);

        animator.SetTrigger("Cast");

        Vector2 spawnPosition =
            (Vector2)transform.position +
            direction * spawnDistance;

        GameObject projectile = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.identity
        );

        projectile
            .GetComponent<MagicProjectile>()
            .Shoot(direction);
    }
}