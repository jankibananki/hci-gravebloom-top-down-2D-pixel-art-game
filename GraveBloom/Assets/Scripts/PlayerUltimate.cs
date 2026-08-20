using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUltimate : MonoBehaviour
{
    [Header("Beam")]
    public GameObject beamPrefab;

    public float spawnOffset = 0.3f;
    public float beamDuration = 0.7f;

    [Header("Casting")]
    public float castDelay = 0.35f;

    [Header("Cooldown")]
    public float cooldownDuration = 25f;

    public Image cooldownImage;

    public Sprite[] cooldownSprites;

    private PlayerAim aim;
    private Animator playerAnimator;

    private bool onCooldown = false;

    private PlayerMovement movement;

    private PlayerSFX playerSFX;

    void Awake()
    {
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<Animator>();
        playerSFX = GetComponent<PlayerSFX>();
    }

    void Start()
    {
        if (cooldownImage != null &&
            cooldownSprites.Length > 0)
        {
            cooldownImage.sprite = cooldownSprites[0];
        }
    }

    void Update()
    {
        if (PauseMenu.IsPaused)
            return;

        if (Keyboard.current == null)
            return;

        // SPACE = ULTIMATE
        if (Keyboard.current.spaceKey.wasPressedThisFrame &&
            !onCooldown)
        {
            StartCoroutine(CastUltimate());
        }
    }

    IEnumerator CastUltimate()
    {
       onCooldown = true;

        movement.SetMovementLocked(true);

        StartCoroutine(UltimateCooldown());

        Vector2 direction = aim.AimDirection;
        Vector2 cardinalDirection = aim.GetCardinalDirection();

        playerAnimator.SetFloat("AimX", cardinalDirection.x);
        playerAnimator.SetFloat("AimY", cardinalDirection.y);
        playerAnimator.SetTrigger("Cast");

        yield return new WaitForSeconds(castDelay);

        Vector2 spawnPosition =
            (Vector2)transform.position +
            direction * spawnOffset;

        GameObject beam = Instantiate(
            beamPrefab,
            spawnPosition,
            Quaternion.identity
        );

        // Rotacija ka mišu
        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg;

        beam.transform.rotation =
            Quaternion.Euler(
                0,
                0,
                angle
            );

        Animator beamAnimator =
            beam.GetComponent<Animator>();

        if (beamAnimator != null)
        {
            beamAnimator.Play(
                "BeamCast",
                0,
                0f
            );
        }

        playerSFX?.PlayUltimate();

        yield return new WaitForSeconds(
            beamDuration
        );
        movement.SetMovementLocked(false);

        if (beamAnimator != null)
        {
            beamAnimator.Play(
                "BeamDestroy",
                0,
                0f
            );
        }

        yield return new WaitForSeconds(0.3f);

        Destroy(beam);
    }

    IEnumerator UltimateCooldown()
    {
        float remaining = cooldownDuration;

        int previousNumber = -1;

        while (remaining > 0f)
        {
            int number =
                Mathf.CeilToInt(remaining);

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

        // Ready ikonica
        if (cooldownImage != null &&
            cooldownSprites.Length > 0)
        {
            cooldownImage.sprite =
                cooldownSprites[0];
        }
    }
}