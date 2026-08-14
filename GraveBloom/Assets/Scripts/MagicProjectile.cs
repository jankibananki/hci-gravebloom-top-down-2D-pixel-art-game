using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 2f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;

        Destroy(gameObject, lifetime);
    }
}