using UnityEngine;

public class MovementController : MonoBehaviour {

    /// <summary>
    /// The movement speed of the player
    /// </summary>
    [SerializeField]
    float speed = 20;

    /// <summary>
    /// The force applied to the player when jumping
    /// </summary>
    [SerializeField]
    float jumpForce = 200;

    /// <summary>
    /// Transform to check if the player is grounded
    /// </summary>
    [SerializeField]
    Transform groundCheck;

    /// <summary>
    /// Detmermines what is ground
    /// </summary>
    [SerializeField]
    LayerMask whatIsGround;

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
    
    /// <summary>
    /// Radius of the overlap circle to determine if grounded 
    /// </summary>
    const float GROUNDED_RADIUS = 0.2f;

    void Start() {
        body = GetComponent<Rigidbody2D>();
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
        grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, GROUNDED_RADIUS, whatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            GameObject collidedGameObject = colliders[i].gameObject;
            if (collidedGameObject != gameObject) {
                grounded = true;
            }
        }

        // Calculate new velocity and apply it to the rigidbody
        Vector2 targetVelocity = new Vector2(horizontalMovement * MOVEMENT_MULTIPLIER * speed * Time.fixedDeltaTime, body.velocity.y);
        body.velocity = Vector2.SmoothDamp(body.velocity, targetVelocity, ref currentVelocity, 0.05f);

        if (jump && grounded) {
            body.AddForce(new Vector2(0, jumpForce));
        }

        jump = false;
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
}
