using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUltimate : MonoBehaviour
{
    [Header("Beam")]
    public GameObject beamPrefab;

    public float spawnOffset = 0.3f;
    public float beamDuration = 0.7f;

    [Header("Casting")]
    public float castDelay = 0.35f;

    [Header("Cooldown")]
    public float cooldown = 8f;

    private PlayerAim aim;
    private Animator playerAnimator;

    private bool onCooldown = false;

    void Awake()
    {
        aim = GetComponent<PlayerAim>();
        playerAnimator =
            GetComponent<Animator>();
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        // SPACE = ULTIMATE
        if (Keyboard.current.spaceKey.wasPressedThisFrame &&
            !onCooldown)
        {
            StartCoroutine(
                CastUltimate()
            );
        }
    }

    IEnumerator CastUltimate()
    {
        onCooldown = true;

        // Zapamtimo TAČAN smer
        // u trenutku pritiska Space-a
        Vector2 direction =
            aim.AimDirection;

        // Ali witch cast animacija ostaje
        // jedna od postojeće 4
        Vector2 cardinalDirection =
            aim.GetCardinalDirection();

        playerAnimator.SetFloat(
            "AimX",
            cardinalDirection.x
        );

        playerAnimator.SetFloat(
            "AimY",
            cardinalDirection.y
        );

        playerAnimator.SetTrigger(
            "Cast"
        );

        // Sačekamo trenutak u cast animaciji
        yield return new WaitForSeconds(
            castDelay
        );

        Vector2 spawnPosition =
            (Vector2)transform.position +
            direction * spawnOffset;

        GameObject beam = Instantiate(
            beamPrefab,
            spawnPosition,
            Quaternion.identity
        );

        // Pošto je originalni beam nacrtan DESNO,
        // rotiramo ga tačno ka mišu
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

        yield return new WaitForSeconds(
            beamDuration
        );

        if (beamAnimator != null)
        {
            beamAnimator.Play(
                "BeamDestroy",
                0,
                0f
            );
        }

        yield return new WaitForSeconds(
            0.3f
        );

        Destroy(beam);

        yield return new WaitForSeconds(
            cooldown
        );

        onCooldown = false;
    }
}