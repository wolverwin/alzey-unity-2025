using Manager;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerMovementController : MonoBehaviour {
        /// <summary>
        /// The rigidbody of the player
        /// </summary>
        [SerializeField]
        private Rigidbody2D body;

        /// <summary>
        /// The movement speed of the player
        /// </summary>
        [Header("Movement"), SerializeField]
        private float speed = 20;

        /// <summary>
        /// The force applied to the player when jumping
        /// </summary>
        [Header("Jumping"), SerializeField]
        private float jumpForce = 20;

        /// <summary>
        /// Whether to anticipate a jump while holding down space, or not
        /// </summary>
        [SerializeField]
        private bool anticipateJump;

        /// <summary>
        /// Transform to check if the player is grounded
        /// </summary>
        [Header("GroundCheck"), SerializeField]
        private Transform groundCheck;

        public Transform GroundCheck => groundCheck;

        /// <summary>
        /// The size of the ground check box to check for
        /// </summary>
        [SerializeField]
        private Vector2 groundCheckSize;

        public Vector2 GroundCheckSize => groundCheckSize;

        /// <summary>
        /// Detmermines what is ground
        /// </summary>
        [SerializeField]
        private LayerMask whatIsGround;

        /// <summary>
        /// The max fall speed of the character
        /// </summary>
        [Header("Gravity"), SerializeField]
        private float maxFallSpeed = 20;

        /// <summary>
        /// How fast the character gets when falling
        /// </summary>
        [SerializeField]
        private float fallSpeedMultiplier = 2;

        /// <summary>
        /// The force applied to the player if getting hurt
        /// </summary>
        [Header("Misc"), SerializeField]
        private float damageJumpForce = 200;

        /// <summary>
        /// The movement action for this player instance
        /// </summary>
        private InputAction moveAction;

        /// <summary>
        /// The jump action for this player instance
        /// </summary>
        private InputAction jumpAction;

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

        /// <summary>
        /// A reference to the GameManager used by this game
        /// </summary>
        private GameManager gameManager;

        /// <summary>
        /// Value to multiply the movement speed by
        /// </summary>
        private const float MOVEMENT_MULTIPLIER = 10f;

        public const float MIN_VERTICAL_VELOCITY = 0.005f;

        private void Start() {
            baseGravity = body.gravityScale;
            gameManager = GameManager.Instance;
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");
            EventManager.OnPlayerHurt += OnPlayerHurt;
            EventManager.OnPlayerRecovered += OnPlayerRecovered;
            EventManager.OnPlayerDied += OnPlayerDied;
        }

        private void Update() {
            DoGroundCheck();
            ApplyFallGravity();

            horizontalMovement = 0;

            if (!blockControls && (gameManager == null || !gameManager.GamePaused)) {
                horizontalMovement = moveAction.ReadValue<Vector2>().x;

                if (anticipateJump) {
                    if (jumpAction.WasPressedThisFrame()) {
                        EventManager.InvokeOnJumpAnticipation();
                    }

                    if (jumpAction.WasReleasedThisFrame()) {
                        jump = true;
                    }
                } else {
                    if (jumpAction.WasPressedThisFrame()) {
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
            Vector2 targetVelocity =
                new Vector2(horizontalMovement * MOVEMENT_MULTIPLIER * speed * Time.fixedDeltaTime, body.linearVelocityY);
            body.linearVelocity = Vector2.SmoothDamp(body.linearVelocity, targetVelocity, ref currentVelocity, 0.05f);

            Jump();
        }

        private void Jump() {
            if (!jump) {
                return;
            }

            if (grounded && !onWall) {
                body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                EventManager.InvokeOnJumpExecuted();
            }

            jump = false;
        }

        /// <summary>
        /// Checks if the character is grounded
        /// </summary>
        private void DoGroundCheck() {
            if (!groundCheck) {
                grounded = true;
                return;
            }

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
            if (body.linearVelocityY < -MIN_VERTICAL_VELOCITY) {
                body.gravityScale = baseGravity * fallSpeedMultiplier;
                body.linearVelocityY = Mathf.Max(body.linearVelocityY, -maxFallSpeed);
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
            body.linearVelocity = Vector2.zero;
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
            if (!groundCheck) {
                return;
            }

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);

            groundCheck.position = Handles.PositionHandle(groundCheck.position, Quaternion.identity);
        }
#endif
    }
}