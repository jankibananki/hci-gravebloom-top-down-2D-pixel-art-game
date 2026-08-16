using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;

    public GameOverUI gameOverUI;
    public int currentHealth;

    public Image healthBarImage;

    // Redom:
    // 0 HP, 1 HP, 2 HP, 3 HP, 4 HP, 5 HP
    public Sprite[] healthSprites;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    //privremeno dok ne dodam skeletoni
    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            TakeDamage(1);
        }

        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            Heal(1);
        }
    }

    public void TakeDamage(int damage)
    {
        // Ako shield postoji i aktivan je,
        // on prima udarac umesto healtha.
        PlayerShield shield = GetComponent<PlayerShield>();

        if (shield != null && shield.IsShieldActive())
        {
            shield.HitShield();
            return;
        }

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth
        );

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth
        );

        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBarImage == null)
            return;

        healthBarImage.sprite = healthSprites[currentHealth];
    }

    void Die()
    {
        PlayerMovement movement =
            GetComponent<PlayerMovement>();

        if (movement != null)
            movement.SetDead();

        Animator animator =
            GetComponent<Animator>();

        if (animator != null)
            animator.SetTrigger("Die");

        if (gameOverUI != null)
            gameOverUI.ShowGameOver();
    }
}