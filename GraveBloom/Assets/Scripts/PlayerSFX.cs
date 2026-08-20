using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField] private AudioClip basicAttackSound;
    [SerializeField] private AudioClip ultimateSound;
    [SerializeField] private AudioClip shieldSound;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayBasicAttack()
    {
        if (audioSource != null && basicAttackSound != null)
            audioSource.PlayOneShot(basicAttackSound);
    }

    public void PlayUltimate()
    {
        if (audioSource != null && ultimateSound != null)
            audioSource.PlayOneShot(ultimateSound);
    }

    public void PlayShield()
    {
        if (audioSource != null && shieldSound != null)
            audioSource.PlayOneShot(shieldSound);
    }
}