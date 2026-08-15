using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShield : MonoBehaviour
{
    public GameObject shieldObject;

    public float breakAnimationTime = 0.5f;
    public float shieldDuration = 7f;

    private Animator shieldAnimator;
    private bool shieldActive = false;

    private Coroutine shieldTimer;

    void Start()
    {
        shieldAnimator = shieldObject.GetComponent<Animator>();

        shieldObject.SetActive(false);
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
        if (shieldActive)
            return;

        shieldActive = true;

        shieldObject.SetActive(true);

        // ISTO ime kao u Animatoru
        shieldAnimator.Play("shieldAppear", 0, 0f);

        shieldTimer = StartCoroutine(AutoDestroyShield());
    }

    IEnumerator AutoDestroyShield()
    {
        yield return new WaitForSeconds(shieldDuration);

        if (shieldActive)
        {
            HitShield();
        }
    }

    public void HitShield()
    {
        if (!shieldActive)
            return;

        shieldActive = false;

        // Zaustavi timer ako ga je neprijatelj razbio ranije
        if (shieldTimer != null)
        {
            StopCoroutine(shieldTimer);
            shieldTimer = null;
        }

        // ISTO ime kao u Animatoru
        shieldAnimator.Play("shieldDestroy", 0, 0f);

        StartCoroutine(DisableShieldAfterBreak());
    }

    IEnumerator DisableShieldAfterBreak()
    {
        yield return new WaitForSeconds(breakAnimationTime);

        shieldObject.SetActive(false);
    }

    public bool IsShieldActive()
    {
        return shieldActive;
    }
}