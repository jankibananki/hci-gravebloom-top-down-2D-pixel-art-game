using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 15;
    private WaveManager waveManager;
    private int currentHealth;
    private bool isDead = false;

    private Animator animator;
    private EnemyAI rangedAI;
    private MeleeEnemyAI meleeAI;
    private BossAI bossAI;
    private Rigidbody2D rb;
    private EnemySFX enemySFX;
    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
    }

    [Header("Drops")]
    public GameObject healthPotionPrefab;

    [Range(0f, 1f)]
    public float healthPotionDropChance = 0.25f;

    private Collider2D[] colliders;

    public void SetWaveManager(WaveManager manager)
    {
        waveManager = manager;
    }
    void Awake()
    {
        currentHealth = maxHealth;

        // radi i ako Animator kasnije prebacim na Visual child
        animator = GetComponentInChildren<Animator>();

        // ako je mage nalazi EnemyAI
        rangedAI = GetComponent<EnemyAI>();

        // ako je obican skeleton nalazi MeleeEnemyAI
        meleeAI = GetComponent<MeleeEnemyAI>();
        //ako je boss nalazi BossAI
        bossAI = GetComponent<BossAI>();

        rb = GetComponent<Rigidbody2D>();

        enemySFX = GetComponent<EnemySFX>();

        //uzima sve collidere i eventualne child hitboxove
        colliders = GetComponentsInChildren<Collider2D>();

        Debug.Log(
            gameObject.name +
            " starting HP: " +
            currentHealth
        );
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        currentHealth =
            Mathf.Clamp(
                currentHealth,
                0,
                maxHealth
            );

        Debug.Log(
            gameObject.name +
            " took " +
            damage +
            " damage. HP: " +
            currentHealth
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (enemySFX != null)
            enemySFX.PlayDeath();

        if (waveManager != null)
        {
            waveManager.EnemyKilled();
        }

        Debug.Log(
            gameObject.name +
            " DIED!"
        );

        // Ako je ranged mage
        if (rangedAI != null)
        {
            rangedAI.SetDead();
        }

        // Ako je melee knight
        if (meleeAI != null)
        {
            meleeAI.SetDead();
        }

        //ako je boss
        if (bossAI != null)
        {
            bossAI.SetDead();
        }

        // Zaustavi ga
        if (rb != null)
        {
            rb.linearVelocity =
                Vector2.zero;
        }

        // Ugasi sve collidere
        // da mrtav enemy vise ne blokira / prima hitove
        foreach (Collider2D col in colliders)
        {
            if (col != null)
                col.enabled = false;
        }

        TryDropHealthPotion();

        // Death animacija
        if (animator != null)
        {
            animator.SetBool(
                "IsMoving",
                false
            );

            animator.SetTrigger(
                "Die"
            );
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    void TryDropHealthPotion()
    {
        if (healthPotionPrefab == null)
            return;

        float roll = Random.value;

        if (roll <= healthPotionDropChance)
        {
            Instantiate(
                healthPotionPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }
}
