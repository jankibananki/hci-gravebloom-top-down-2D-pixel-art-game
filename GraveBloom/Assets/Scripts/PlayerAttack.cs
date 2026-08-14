using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float spawnDistance = 0.6f;

    private PlayerMovement movement;
    private Animator animator;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Cast();
        }
    }

    void Cast()
    {
        Vector2 direction = movement.LastDirection;

        animator.SetTrigger("Cast");

        Vector2 spawnPosition =
            (Vector2)transform.position + direction * spawnDistance;

        GameObject projectile = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.identity
        );

        projectile.GetComponent<MagicProjectile>().Shoot(direction);
    }
}