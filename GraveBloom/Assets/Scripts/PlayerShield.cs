using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShield : MonoBehaviour
{
    [Header("Shield")]
    public GameObject shieldObject;
    public float shieldDuration = 4f;
    public float breakAnimationTime = 0.5f;

    [Header("Cooldown")]
    public float cooldownDuration = 15f;
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

        if (cooldownImage != null && cooldownSprites.Length > 0)
        {
            cooldownImage.sprite = cooldownSprites[0];
        }
    }

    void Update()
    {
        if (Mouse.current == null)
            return;

        // DESNI KLIK = shield
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            ActivateShield();
        }
    }

    void ActivateShield()
    {
        if (shieldActive || onCooldown)
            return;

        shieldActive = true;

        shieldObject.SetActive(true);

        shieldAnimator.Play("shieldAppear", 0, 0f);

        // Shield traje određeno vreme
        shieldTimer = StartCoroutine(ShieldLifetime());

        // COOLDOWN KREĆE ODMAH NAKON CASTA
        StartCoroutine(ShieldCooldown());
    }

    IEnumerator ShieldLifetime()
    {
        yield return new WaitForSeconds(shieldDuration);

        shieldTimer = null;

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
        if (!shieldActive)
            return;

        shieldActive = false;

        if (shieldTimer != null)
        {
            StopCoroutine(shieldTimer);
            shieldTimer = null;
        }

        shieldAnimator.Play("shieldDestroy", 0, 0f);

        StartCoroutine(DisableShieldAfterBreak());

        // OVO VIŠE NIJE OVDE:
        // StartCoroutine(ShieldCooldown());
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

        while (remaining > 0f)
        {
            int number = Mathf.CeilToInt(remaining);

            if (number != previousNumber)
            {
                previousNumber = number;

                if (cooldownImage != null &&
                    cooldownSprites.Length > 1)
                {
                    number = Mathf.Clamp(
                        number,
                        1,
                        cooldownSprites.Length - 1
                    );

                    cooldownImage.sprite =
                        cooldownSprites[number];
                }
            }

            remaining -= Time.deltaTime;

            yield return null;
        }

        onCooldown = false;

        if (cooldownImage != null &&
            cooldownSprites.Length > 0)
        {
            cooldownImage.sprite =
                cooldownSprites[0];
        }
    }

    public bool IsShieldActive()
    {
        return shieldActive;
    }
}