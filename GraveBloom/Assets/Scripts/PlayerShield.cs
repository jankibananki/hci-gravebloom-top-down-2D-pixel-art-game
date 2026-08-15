using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShield : MonoBehaviour
{
    [Header("Shield")]
    public GameObject shieldObject;
    public float shieldDuration = 7f;
    public float breakAnimationTime = 0.5f;

    [Header("Cooldown")]
    public float cooldownDuration = 5f;
    public Image cooldownImage;

    // 0 = ready/prazno
    // 1 = broj 1
    // 2 = broj 2
    // ...
    // 5 = broj 5
    public Sprite[] cooldownSprites;

    private Animator shieldAnimator;

    private bool shieldActive = false;
    private bool onCooldown = false;

    private Coroutine shieldTimer;

    void Start()
    {
        shieldAnimator = shieldObject.GetComponent<Animator>();

        shieldObject.SetActive(false);

        // Na početku je shield spreman
        cooldownImage.sprite = cooldownSprites[0];
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            ActivateShield();
        }
    }

    void ActivateShield()
    {
        // Ne može novi shield dok je aktivan
        // niti dok traje cooldown
        if (shieldActive || onCooldown)
            return;

        shieldActive = true;

        shieldObject.SetActive(true);

        shieldAnimator.Play("shieldAppear", 0, 0f);

        shieldTimer = StartCoroutine(ShieldLifetime());
    }

    IEnumerator ShieldLifetime()
    {
        yield return new WaitForSeconds(shieldDuration);

        if (shieldActive)
        {
            DestroyShield();
        }
    }

    public void HitShield()
    {
        if (!shieldActive)
            return;

        DestroyShield();
    }

    void DestroyShield()
    {
        shieldActive = false;

        if (shieldTimer != null)
        {
            StopCoroutine(shieldTimer);
            shieldTimer = null;
        }

        shieldAnimator.Play("shieldDestroy", 0, 0f);

        StartCoroutine(DisableShieldAfterBreak());
        StartCoroutine(ShieldCooldown());
    }

    IEnumerator DisableShieldAfterBreak()
    {
        yield return new WaitForSeconds(breakAnimationTime);

        shieldObject.SetActive(false);
    }

    IEnumerator ShieldCooldown()
    {
        onCooldown = true;

        float remaining = cooldownDuration;
        int previousNumber = -1;

        while (remaining > 0)
        {
            int number = Mathf.CeilToInt(remaining);

            if (number != previousNumber)
            {
                previousNumber = number;

                // Za slučaj da kasnije staviš drugi cooldown
                number = Mathf.Clamp(
                    number,
                    1,
                    cooldownSprites.Length - 1
                );

                cooldownImage.sprite = cooldownSprites[number];
            }

            remaining -= Time.deltaTime;

            yield return null;
        }

        onCooldown = false;

        // Prazno prozorče = shield spreman
        cooldownImage.sprite = cooldownSprites[0];
    }

    public bool IsShieldActive()
    {
        return shieldActive;
    }
}