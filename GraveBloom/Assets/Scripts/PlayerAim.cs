using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Camera cam;

    public Vector2 AimDirection { get; private set; } = Vector2.right;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Mouse.current == null || cam == null)
            return;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition =
            cam.ScreenToWorldPoint(
                new Vector3(
                    mouseScreenPosition.x,
                    mouseScreenPosition.y,
                    -cam.transform.position.z
                )
            );

        AimDirection =
            ((Vector2)mouseWorldPosition - (Vector2)transform.position).normalized;
    }

    // Ovo koristimo SAMO da izaberemo
    // Front / Back / Left / Right cast animaciju.
    public Vector2 GetCardinalDirection()
    {
        if (Mathf.Abs(AimDirection.x) > Mathf.Abs(AimDirection.y))
        {
            return new Vector2(
                Mathf.Sign(AimDirection.x),
                0
            );
        }

        return new Vector2(
            0,
            Mathf.Sign(AimDirection.y)
        );
    }
}