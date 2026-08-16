using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 4f;
    public int damage = 1;

    public float maxRange = 6f;

private Vector2 startPosition;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Shoot(Vector2 direction)
    {
        startPosition = transform.position;
        direction = direction.normalized;

        rb.linearVelocity = direction * speed;

        float angle =
            Mathf.Atan2(direction.y, direction.x)
            * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0, 0, angle+180f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health =
                other.GetComponent<PlayerHealth>();

            if (health != null)
                health.TakeDamage(damage);

            Destroy(gameObject);
        }
    }

    void Update()
{
    float distanceTravelled =
        Vector2.Distance(startPosition, transform.position);

    if (distanceTravelled >= maxRange)
        Destroy(gameObject);
}
}