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
        direction = direction.normalized;

        rb.linearVelocity = direction * speed;

        // Sprite je originalno nacrtan da leti DESNO
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifetime);
    }
}