using Manager;
using UnityEditor;
using UnityEngine;

namespace Player {
    public class PlayerMovementController : MonoBehaviour {

        /// <summary>
        /// The rigidbody of the player
        /// </summary>
        [SerializeField]
        private Rigidbody2D body;

        [Header("Movement")]
        /// <summary>
        /// The movement speed of the player
        /// </summary>
        [SerializeField]
        private float speed = 20;

        [Header("Jumping")]
        /// <summary>
        /// The force applied to the player when jumping
        /// </summary>
        [SerializeField]
        private float jumpForce = 200;

        /// <summary>
        /// Whether to anticipate a jump while holding down space, or not
        /// </summary>
        [SerializeField]
        private bool anticipateJump;

        [Header("GroundCheck")]
        /// <summary>
        /// Transform to check if the player is grounded
        /// </summary>
        [SerializeField]
        private Transform groundCheck;

        public Transform GroundCheck { get { return groundCheck; } }

        /// <summary>
        /// The size of the ground check box to check for
        /// </summary>
        [SerializeField]
        private Vector2 groundCheckSize;

        public Vector2 GroundCheckSize { get { return groundCheckSize; } }

        /// <summary>
        /// Detmermines what is ground
        /// </summary>
        [SerializeField]
        private LayerMask whatIsGround;

        [Header("Gravity")]
        /// <summary>
        /// The max fall speed of the character
        /// </summary>
        [SerializeField]
        private float maxFallSpeed = 20;

        /// <summary>
        /// How fast the character gets when falling
        /// </summary>
        [SerializeField]
        private float fallSpeedMultiplier = 2;

        [Header("Misc")]
        /// <summary>
        /// The force applied to the player if getting hurt
        /// </summary>
        [SerializeField]
        private float damageJumpForce = 200;

        /// <summary>
        /// The horizontal movement from the input system
        /// </summary>
        private float horizontalMovement;

        /// <summary>
        /// If true, all controls are blocked
        /// </summary>
        private bool blockControls;

        /// <summary>
        /// Whether the player should jump in the next fixed update or not
        /// </summary>
        private bool jump;

        /// <summary>
        /// Whether the player is grounded or not
        /// </summary>
        private bool grounded;

        /// <summary>
        /// The base gravity we get from the rigidbody at start
        /// </summary>
        private float baseGravity;

        /// <summary>
        /// Whether the player is currently on a wall or not
        /// </summary>
        private bool onWall;

        /// <summary>
        /// Whether the player currently faces right or left
        /// </summary>
        private bool facingRight = true;

        /// <summary>
        /// The current velocity of the player
        /// </summary>
        private Vector2 currentVelocity = Vector2.zero;
        private GameManager gameManager;

        /// <summary>
        /// Value to multiply the movement speed by
        /// </summary>
        private const float MOVEMENT_MULTIPLIER = 10f;

        public const float MIN_VERTICAL_VELOCITY = 0.005f;

        private void Start() {
            baseGravity = body.gravityScale;
            gameManager = GameManager.Instance;
            EventManager.OnPlayerHurt += OnPlayerHurt;
            EventManager.OnPlayerRecovered += OnPlayerRecovered;
            EventManager.OnPlayerDied += OnPlayerDied;
        }

        private void Update() {
            DoGroundCheck();
            ApplyFallGravity();

            horizontalMovement = 0;

            if (!blockControls && (gameManager == null || !gameManager.GamePaused)) {
                horizontalMovement = Input.GetAxisRaw("Horizontal");


                if (anticipateJump) {
                    if (Input.GetButtonDown("Jump")) {
                        EventManager.InvokeOnJumpAnticipation();
                    }

                    if (Input.GetButtonUp("Jump")) {
                        jump = true;
                    }
                } else {
                    if (Input.GetButtonDown("Jump")) {
                        jump = true;
                    }
                }
            }

            // Flip the player asset if needed
            if ((horizontalMovement > 0 && !facingRight) || (horizontalMovement < 0 && facingRight)) {
                Flip();
            }
        }

        private void FixedUpdate() {
            // Calculate new velocity and apply it to the rigidbody
            Vector2 targetVelocity = new Vector2(horizontalMovement * MOVEMENT_MULTIPLIER * speed * Time.fixedDeltaTime, body.velocity.y);
            body.velocity = Vector2.SmoothDamp(body.velocity, targetVelocity, ref currentVelocity, 0.05f);

            Jump();
        }

        private void Jump() {
            if (jump == false) {
                return;
            }

            if (grounded && !onWall) {
                body.AddForce(new Vector2(0, jumpForce));
                EventManager.InvokeOnJumpExecuted();
            }

            jump = false;
        }

        /// <summary>
        /// Checks if the character is grounded
        /// </summary>
        private void DoGroundCheck() {
            grounded = false;

            // The player is grounded if a boxcast to the groundcheck position hits anything designated as ground
            Collider2D collider2d = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, whatIsGround);
            if (collider2d != null && collider2d.gameObject != gameObject) {
                grounded = true;
            }
        }

        /// <summary>
        /// Applies fall gravity if the character is falling
        /// </summary>
        private void ApplyFallGravity() {
            if (body.velocity.y < -MIN_VERTICAL_VELOCITY) {
                body.gravityScale = baseGravity * fallSpeedMultiplier;
                body.velocity = new Vector2(body.velocity.x, Mathf.Max(body.velocity.y, -maxFallSpeed));

            } else {
                body.gravityScale = baseGravity;
            }
        }

        /// <summary>
        /// Flip the player asset on the x-axis
        /// </summary>
        private void Flip() {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            facingRight = !facingRight;

            EventManager.InvokeOnCharacterFlip();
        }

        /// <summary>
        /// Triggered when the player gets hurt
        /// </summary>
        private void OnPlayerHurt(GameObject source) {

            Vector3 away = (transform.position - source.transform.position).normalized * damageJumpForce;
            Vector2 force = new Vector2(away.x, away.y);

            // Make the player jump away when getting hurt
            body.velocity = Vector2.zero;
            body.AddForce(force);
            blockControls = true;
        }

        /// <summary>
        /// Triggered when the player recovered
        /// </summary>
        private void OnPlayerRecovered() {
            blockControls = false;
        }

        /// <summary>
        /// Triggered when the player died
        /// </summary>
        private void OnPlayerDied() {
            blockControls = true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);

            groundCheck.position = Handles.PositionHandle(groundCheck.position, Quaternion.identity);
        }
#endif
    }
}
