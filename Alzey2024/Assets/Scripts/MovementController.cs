using UnityEngine;

public class MovementController : MonoBehaviour {

    [Header("Movement")]
    /// <summary>
    /// The movement speed of the player
    /// </summary>
    [SerializeField]
    float speed = 20;

    [Header("Jumping")]
    /// <summary>
    /// The force applied to the player when jumping
    /// </summary>
    [SerializeField]
    float jumpForce = 200;

    [Header("GroundCheck")]
    /// <summary>
    /// Transform to check if the player is grounded
    /// </summary>
    [SerializeField]
    Transform groundCheck;

    /// <summary>
    /// The size of the ground check box to check for
    /// </summary>
    [SerializeField]
    Vector2 groundCheckSize;

    /// <summary>
    /// Detmermines what is ground
    /// </summary>
    [SerializeField]
    LayerMask whatIsGround;

    [Header("Gravity")]
    /// <summary>
    /// The max fall speed of the character
    /// </summary>
    [SerializeField]
    float maxFallSpeed = 20;

    /// <summary>
    /// How fast the character gets when falling
    /// </summary>
    [SerializeField]
    float fallSpeedMultiplier = 2;

    [Header("WallCheck")]
    /// <summary>
    /// All transforms to check if the player touches a wall
    /// </summary>
    [SerializeField]
    Transform wallCheck;

    /// <summary>
    /// The size of the wall check box to check for
    /// </summary>
    [SerializeField]
    Vector2 wallCheckSize;

    /// <summary>
    /// The horizontal movement from the input system
    /// </summary>
    float horizontalMovement;

    /// <summary>
    /// Whether the player should jump in the next fixed update or not
    /// </summary>
    bool jump;

    /// <summary>
    /// Whether the player is grounded or not
    /// </summary>
    bool grounded;

    /// <summary>
    /// The base gravity we get from the rigidbody at start
    /// </summary>
    float baseGravity;

    /// <summary>
    /// Whether the player is currently on a wall or not
    /// </summary>
    bool onWall;

    /// <summary>
    /// Whether the player currently faces right or left
    /// </summary>
    bool facingRight = true;

    /// <summary>
    /// The rigidbody of the player
    /// </summary>
    Rigidbody2D body;

    /// <summary>
    /// The current velocity of the player
    /// </summary>
    Vector2 currentVelocity = Vector2.zero;

    /// <summary>
    /// Value to multiply the movement speed by
    /// </summary>
    const float MOVEMENT_MULTIPLIER = 10f;

    const float MIN_FALL_VELOCITY = -0.005f;

    void Start() {
        body = GetComponent<Rigidbody2D>();
        baseGravity = body.gravityScale;
    }

    void Update() {
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }

        // Flip the player asset if needed
        if ((horizontalMovement > 0 && !facingRight) || (horizontalMovement < 0 && facingRight)) {
            Flip();
        }
    }

    void FixedUpdate() {
        DoGroundCheck();
        DoWallCheck();
        ApplyFallGravity();
        
        // Calculate new velocity and apply it to the rigidbody
        Vector2 targetVelocity = new Vector2(horizontalMovement * MOVEMENT_MULTIPLIER * speed * Time.fixedDeltaTime, body.velocity.y);
        body.velocity = Vector2.SmoothDamp(body.velocity, targetVelocity, ref currentVelocity, 0.05f);

        if (jump && (grounded || onWall)) {
            body.AddForce(new Vector2(0, jumpForce));
        }

        jump = false;
    }

    /// <summary>
    /// Checks if the character is grounded
    /// </summary>
    void DoGroundCheck() {
        grounded = false;

        // The player is grounded if a boxcast to the groundcheck position hits anything designated as ground
        Collider2D collider2d = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, whatIsGround);
        if (collider2d != null && collider2d.gameObject != gameObject) {
            grounded = true;
        }
    }

    /// <summary>
    /// Checks if the character is sliding on a wall
    /// </summary>
    void DoWallCheck() {
        onWall = false;

        Collider2D collider2d = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, whatIsGround);
        if (!grounded && collider2d != null && collider2d.gameObject != gameObject) {
            onWall = true;
        }
    }

    void ApplyFallGravity() {
        if (body.velocity.y < MIN_FALL_VELOCITY) {
            body.gravityScale = baseGravity * fallSpeedMultiplier;
            body.velocity = new Vector2(body.velocity.x, Mathf.Max(body.velocity.y, -maxFallSpeed));

        } else {
            body.gravityScale = baseGravity;
        }
    }

    /// <summary>
    /// Flip the player asset on the x-axis
    /// </summary>
    void Flip() {
        if (facingRight) {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            facingRight = false;
        } else {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            facingRight = true;
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
