using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    // Kad se igra pokrene, gleda dole
    private Vector2 lastDirection = Vector2.down;

    public Vector2 LastDirection => lastDirection;
    
    private bool movementLocked = false;

    public void SetMovementLocked(bool locked)
    {
        movementLocked = locked;

        if (locked)
        {
            movement = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        animator.SetFloat("MoveX", lastDirection.x);
        animator.SetFloat("MoveY", lastDirection.y);
        animator.SetBool("IsMoving", false);
    }

    void Update()
    {
        movement = Vector2.zero;

        if (movementLocked)
        {
            movement = Vector2.zero;
            return;
        }

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.wKey.isPressed)
            movement.y += 1;

        if (Keyboard.current.sKey.isPressed)
            movement.y -= 1;

        if (Keyboard.current.aKey.isPressed)
            movement.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            movement.x += 1;

        movement = movement.normalized;

        bool isMoving = movement != Vector2.zero;

        animator.SetBool("IsMoving", isMoving);

        // Menjamo smer samo dok se kreće
        if (isMoving)
        {
            // Ako ide dijagonalno, bira dominantni smer
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                lastDirection = new Vector2(Mathf.Sign(movement.x), 0);
            }
            else
            {
                lastDirection = new Vector2(0, Mathf.Sign(movement.y));
            }

            animator.SetFloat("MoveX", lastDirection.x);
            animator.SetFloat("MoveY", lastDirection.y);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }
}