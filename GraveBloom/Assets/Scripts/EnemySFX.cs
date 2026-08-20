using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip deathSound;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayAttack()
    {
        if (audioSource != null && attackSound != null)
            audioSource.PlayOneShot(attackSound);
    }

    public void PlayDeath()
    {
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);
    }
}