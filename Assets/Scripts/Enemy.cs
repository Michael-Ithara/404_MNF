using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Player Reference")]
    private Transform player; // Reference to the player

    [Header("Movement")]
    public float chaseSpeed = 3f; // Speed at which the enemy chases the player
    public float jumpforce = 5f; // Force applied when the enemy jumps

    [Header("Ground Check")]
    public Transform groundCheckPos; // Empty GameObject at the enemy's feet
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f); // Size of the box for ground detection
    public LayerMask groundLayer; // Layer mask for ground detection
    private bool isGrounded; // Is the enemy on the ground?

    private Rigidbody2D rb; // Reference to the Rigidbody2D
    private bool shouldJump; // Should the enemy jump?

    public int damage = 1; // Damage dealt by the enemy
    public int maxHealth = 3; // Maximum health of the enemy
    private int currentHealth; // Current health of the enemy
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer for visual feedback
    private Color Ogcolor;

public PlayerHealth playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer component
        player = GameObject.FindWithTag("Player").GetComponent<Transform>(); // Find the player by tag
        currentHealth = maxHealth; // Initialize current health
        Ogcolor = spriteRenderer.color;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // Check if grounded
        isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer);

        // Player direction
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        // Apply horizontal velocity (even in air)
        rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);

        // Detect gap ahead (diagonally down)
        Vector2 forwardDown = new Vector2(direction, -1).normalized;
        RaycastHit2D gapAhead = Physics2D.Raycast(transform.position, forwardDown, 1.5f, groundLayer);

        // Detect if player is above
        bool isPlayerAbove = player.position.y > transform.position.y;

        // Decide to jump
        if (isGrounded)
        {
            if (!gapAhead.collider || isPlayerAbove)
            {
                shouldJump = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isGrounded && shouldJump)
        {
            shouldJump = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset vertical speed
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce health by damage amount
        StartCoroutine(FlashWhite()); // Flash red on damage
        if (currentHealth <= 0)
        {
            Die(); // Call die method if health is zero or less
        }
    }
    
    private IEnumerator FlashWhite()
    {
        spriteRenderer.color = Color.white; // Change color to white for flash effect
        yield return new WaitForSeconds(0.1f); // Wait for a short duration
        spriteRenderer.color = Ogcolor; // Restore original color
    
    }
    void Die()
    {
        // Handle enemy death (e.g., play animation, destroy object)
        Destroy(gameObject); // Destroy the enemy GameObject
    }

// void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
// 	    Debug.Log("Collided with player");
//             playerHealth.TakeDamage(1);
//         }
//     }
    private void OnDrawGizmosSelected()
    {
        // Visualize ground check area
        if (groundCheckPos != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        }
    }
}
