using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerHealth health =
            other.GetComponent<PlayerHealth>();

        if (health == null)
            return;

        health.Heal(healAmount);

        Destroy(gameObject);
    }
}