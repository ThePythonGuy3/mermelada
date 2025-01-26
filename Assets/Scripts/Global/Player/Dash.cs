using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashForce = 25f; // Force applied during the dash
    public float dashDuration = 0.5f; // How long the dash lasts
    public float dashCooldown = 1f; // Cooldown between dashes

    private Rigidbody2D rb;
    private bool isDashing = false;
    private float dashTimeRemaining = 0f;
    private float dashCooldownRemaining = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Handle dash cooldown
        if (dashCooldownRemaining > 0)
        {
            dashCooldownRemaining -= Time.deltaTime;
        }

        // Check for dash input (e.g., right mouse button) and cooldown
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownRemaining <= 0 && !isDashing)
        {
            StartDash();
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            DashMovement();
        }
    }

    void StartDash()
    {
        // Set up dash state
        isDashing = true;
        dashTimeRemaining = dashDuration;

        // Calculate dash direction (towards the mouse)
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dashDirection = (mousePosition - (Vector2)transform.position).normalized;

        // Apply force for the dash
        rb.linearVelocity = dashDirection * dashForce;

        // Set cooldown
        dashCooldownRemaining = dashCooldown;
    }

    void DashMovement()
    {
        // Reduce dash time remaining
        dashTimeRemaining -= Time.fixedDeltaTime;

        // Stop dash when time is up
        if (dashTimeRemaining <= 0)
        {
            isDashing = false;
            rb.linearVelocity = Vector2.zero; // Stop movement after the dash
        }
    }
}
