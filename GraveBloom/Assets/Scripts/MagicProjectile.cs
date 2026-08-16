using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 2f;

    private Rigidbody2D rb;

    public float maxRange = 10f;
    public float fadeStartPercent = 0.85f;

    private Vector2 startPosition;
    private SpriteRenderer[] spriteRenderers;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderers =
        GetComponentsInChildren<SpriteRenderer>();
    }

   public void Shoot(Vector2 direction)
    {
        direction = direction.normalized;

        rb.linearVelocity = direction * speed;

        startPosition = transform.position;

        rb.linearVelocity =
            direction.normalized * speed;

        // Sprite je originalno nacrtan da leti DESNO
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

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

    // Počni fade tek pred kraj range-a
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

    // Stigao do kraja range-a
    if (distanceTravelled >= maxRange)
    {
        Destroy(gameObject);
    }
}
}