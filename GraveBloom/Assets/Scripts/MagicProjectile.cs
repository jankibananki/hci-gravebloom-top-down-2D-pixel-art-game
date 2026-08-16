using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;
    public float lifetime = 2f;
    public float maxRange = 10f;

    [Header("Damage")]
    public int damage = 1;

    // Koliko projectile uđe u enemija
    // pre nego što nestane
    public float hitPenetration = 0.3f;

    [Header("Fade")]
    public float fadeStartPercent = 0.85f;

    private Rigidbody2D rb;
    private Collider2D projectileCollider;

    private Vector2 startPosition;
    private Vector2 hitStartPosition;

    private SpriteRenderer[] spriteRenderers;

    private bool hasHitEnemy = false;
    private EnemyHealth enemyHit;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        projectileCollider = GetComponent<Collider2D>();

        spriteRenderers =
            GetComponentsInChildren<SpriteRenderer>();
    }

    public void Shoot(Vector2 direction)
    {
        direction = direction.normalized;

        startPosition = transform.position;

        rb.linearVelocity =
            direction * speed;

        // Sprite je nacrtan da gleda desno
        float angle =
            Mathf.Atan2(direction.y, direction.x)
            * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Ako smo već dotakli enemija,
        // pusti projectile još malo unutra
        if (hasHitEnemy)
        {
            float penetration =
                Vector2.Distance(
                    hitStartPosition,
                    transform.position
                );

            if (penetration >= hitPenetration)
            {
                if (enemyHit != null)
                    enemyHit.TakeDamage(damage);

                Destroy(gameObject);
            }

            return;
        }

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
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitEnemy)
            return;

        EnemyHealth enemy =
            other.GetComponentInParent<EnemyHealth>();

        if (enemy == null)
            return;

        hasHitEnemy = true;
        enemyHit = enemy;

        hitStartPosition = transform.position;

        // Da ne registruje još 37 collidera
        // dok ulazi u skeletona
        if (projectileCollider != null)
            projectileCollider.enabled = false;
    }
}