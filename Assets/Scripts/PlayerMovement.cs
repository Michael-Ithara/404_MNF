using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // Import the Input System namespace

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb; 
    public Animator animator;
    public ParticleSystem smokeFx; // Reference to the smoke effect particle system

    [Header("Movement")]
    bool isFacingRight = false;
    public float moveSpeed = 5f; 
    float horizontalMovement; 
    bool isWalking;             // NEW: Flag to check if the player is walking
    bool audioPlayed;           // NEW: Flag to prevent spamming footsteps

    [Header("Jumping")]
    public float jumpPower = 10f; 

    [Header("Ground Check")]
    public Transform groundCheckPos; 
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f); 
    public LayerMask groundLayer;
    bool isGrounded;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 10f; 
    public float fallgravityMultiplier = 2f;

    [Header("Wall Check")]
    public Transform wallCheckPos; // Transform to check for walls
    public Vector2 wallCheckSize = new Vector2(0.1f, 0.5f); 
    public LayerMask wallLayer; // Layer mask to identify walls

    [Header("WallMovement")]
    public float wallSlideSpeed = 2f; 
    bool isWallSliding; 

    // wall Jumping
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f); 

    void Start()
    {
    }

    void Update()
    {
        GroundCheck();
        ProcessGravity(); // Call the ProcessGravity method to apply gravity
        // ProcessWallSlide();
        // ProcesswallJump();

        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            Flip();
        }

        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);

        HandleWalkingAudio(); // Check for footstep sound
    }

    private void GroundCheck()
    {
        // Check if the player is grounded by casting a box at the ground check position
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            isGrounded = true; // Set the grounded flag to true
            IdleFx(); // Play idle effect when grounded
        }
        else
        {
            isGrounded = false; // Reset the grounded flag
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        isWalking = horizontalMovement != 0 && isGrounded; // Update walking flag
        if (isWalking)
        {
            audioPlayed = false; // Reset audio flag when starting to walk
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                JumpFx();

                // Play the jump sound
                SoundManager.instance.PlayJump(); // Play the jump sound
            }
            else if (context.canceled && rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                JumpFx();
            }
        }

        // Wall Jump
        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpPower.x * wallJumpDirection, wallJumpPower.y);
            wallJumpTimer = 0f;
            animator.SetTrigger("WallJump");

            // Force Flip
            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }
        }
    }

    private void JumpFx()
    {
        animator.SetTrigger("Jump");
        smokeFx.Play();
    }

    private void IdleFx()
    {
        animator.SetTrigger("idle");
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

            if (rb.linearVelocity.y == 0)
            {
                smokeFx.Play();
            }
        }
    }

    private void ProcessGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallgravityMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void HandleWalkingAudio()
    {
        if (isWalking && !audioPlayed)
        {
            SoundManager.instance.PlayStep();
            audioPlayed = true;
        }
        else if (!isWalking)
        {
            audioPlayed = false; // Reset flag when not walking
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
