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
       // ProcessWallSlide(); // Call the ProcessWallSlide method to handle wall sliding
       // ProcesswallJump(); // Call the ProcessWallJump method to handle wall jumping


        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            Flip();
        }
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
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
    }
	
    public void Jump(InputAction.CallbackContext context)
    {

        if (isGrounded) // Check if the player is grounded
        {
            if (context.performed) //&& Mathf.Abs(rb.velocity.y) < 0.001f) 
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                JumpFx(); // Play jump effect
            }
            else if (context.canceled && rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                JumpFx(); // Play jump effect
            }
        }
        // Wall Jump
        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity= new Vector2(wallJumpPower.x * wallJumpDirection, wallJumpPower.y); // Apply wall jump force
            wallJumpTimer = 0f; // Reset the wall jump timer
            animator.SetTrigger("WallJump"); // Trigger the wall jump animation

            //Force Flip
            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f; // Invert the x scale to flip the player
                transform.localScale = ls; // Flip the player if the wall jump direction is different from the current facing direction
            }
        // Invoke(nameof(CancelWallJump), wallJumpTime +0.1f); // Cancel wall jump after a short delay    
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
            // Flip the player's facing direction
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f; // Invert the x scale to flip the player
            transform.localScale = ls;
            
            if (rb.linearVelocity.y == 0)
            {
                smokeFx.Play();
            }
        }
    }

/* private bool wallCheck()
{
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0f, wallLayer);
     }
*/
    private void ProcessGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallgravityMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed)); // Increase gravity when falling
        }
        else
        {
            rb.gravityScale = baseGravity; 
        }
    }
/*
private void ProcessWallSlide()
    {
        // Check if the player is touching a wall
        if (!isGrounded && wallCheck() && horizontalMovement != 0)
        {
            isWallSliding = true; // Set the wall sliding flag
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed)); // Apply wall slide speed
        }
        else
        {
            isWallSliding = false; // Reset the wall sliding flag
        }
            
    }
private void ProcesswallJump()
    {
    if (isWallSliding)
    {
        isWallJumping = false;
        wallJumpDirection = -transform.localScale.x; // Set the wall jump direction based on the player's facing direction
        wallJumpTimer = wallJumpTime;
           
        CancelInvoke(nameof(CancelWallJump)); // Cancel any previous wall jump cancellation
    }
    else if (wallJumpTimer > 0f)
    {
        wallJumpTimer -= Time.deltaTime; // Decrease the wall jump timer
    }
    }    
private void CancelWallJump()
{
    isWallJumping = false; // Reset the wall jumping flag
}

*/
    private void OnDrawGizmosSelected()
    {
        // Draw a green rectangle to visualize the ground check area in the editor
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
