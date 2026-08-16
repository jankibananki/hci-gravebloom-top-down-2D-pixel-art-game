using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;
    public float lifetime = 2f;
    public float maxRange = 10f;

    [Header("Damage")]
    public int damage = 1;

    [Header("Fade")]
    public float fadeStartPercent = 0.85f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private SpriteRenderer[] spriteRenderers;

    private bool hasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderers =
            GetComponentsInChildren<SpriteRenderer>();
    }

    public void Shoot(Vector2 direction)
    {
        direction = direction.normalized;

        startPosition = transform.position;

        rb.linearVelocity =
            direction * speed;

        float angle =
            Mathf.Atan2(direction.y, direction.x)
            * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        float distanceTravelled =
            Vector2.Distance(
                startPosition,
                transform.position
            );

        float fadeStartDistance =
            maxRange * fadeStartPercent;

        if (distanceTravelled >= fadeStartDistance)
        {
            float fadeProgress =
                Mathf.InverseLerp(
                    fadeStartDistance,
                    maxRange,
                    distanceTravelled
                );

            float alpha = 1f - fadeProgress;

            foreach (SpriteRenderer sr in spriteRenderers)
            {
                Color color = sr.color;
                color.a = alpha;
                sr.color = color;
            }
        }

        if (distanceTravelled >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        EnemyHealth enemy =
            other.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            hasHit = true;

            Debug.Log("BASIC HIT ENEMY!");

            enemy.TakeDamage(damage);

            // magic sprite odmah nestaje
            Destroy(gameObject);

            return;
        }
    }
}