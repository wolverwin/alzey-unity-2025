using Manager;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour {
        [SerializeField]
        private Rigidbody2D body;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private Animator animator;

        [SerializeField, Range(0, 1)]
        private float invincibleAlpha = 0.5f;

        [SerializeField]
        private bool useSleepAnimation;

        [SerializeField, Tooltip("Time until sleep animation is played")]
        private float sleepTimer = 2.5f;

        [Header("Animation Parameters"), SerializeField]
        private string speedParameter = "Speed";

        [SerializeField]
        private string jumpingParameter = "Jumping";

        [SerializeField]
        private string anticipateJumpParameter = "AnticipateJump";

        [SerializeField]
        private string fallingParameter = "Falling";

        [SerializeField]
        private string hurtParameter = "Hurt";

        [SerializeField]
        private string sleepParameter = "Sleep";

        [SerializeField]
        private string dieParameter = "Die";

        private GameManager gameManager;
        private float initialAlpha;
        private float timeUntilSleep;


        private const float VELOCITY_MIN = 0.01f;

        private void Start() {
            gameManager = GameManager.Instance;
            initialAlpha = spriteRenderer.color.a;

            EventManager.OnPlayerHurt += TriggerHurtAnimation;
            EventManager.OnPlayerRecovered += StopHurtAnimation;
            EventManager.OnPlayerInvincible += OnPlayerInvincible;
            EventManager.OnPlayerNotInvincible += OnPlayerNotInvincible;
            EventManager.OnPlayerDied += OnPlayerDied;
            EventManager.OnJumpAnticipation += OnJumpAnticipation;
            EventManager.OnJumpExecuted += OnJumpExecuted;
        }

        private void Update() {
            Vector2 velocity = body.linearVelocity;

            if (
                !useSleepAnimation ||
                velocity.x > VELOCITY_MIN || velocity.x < -VELOCITY_MIN ||
                velocity.y > VELOCITY_MIN || velocity.y < -VELOCITY_MIN
            ) {
                timeUntilSleep = 0;
            } else {
                timeUntilSleep += Time.deltaTime;
            }

            if (timeUntilSleep >= sleepTimer) {
                animator.SetBool(sleepParameter, true);
                return;
            }

            animator.SetBool(sleepParameter, false);
            animator.SetFloat(speedParameter, Mathf.Abs(velocity.x));

            bool isJumping = false;
            bool isFalling = false;

            if (body.linearVelocityY > PlayerMovementController.MIN_VERTICAL_VELOCITY) {
                isJumping = true;
            } else if (body.linearVelocityY < -PlayerMovementController.MIN_VERTICAL_VELOCITY) {
                isFalling = true;
            }

            animator.SetBool(jumpingParameter, isJumping);
            animator.SetBool(fallingParameter, isFalling);
        }

        private void TriggerHurtAnimation(GameObject source) {
            animator.SetBool(hurtParameter, true);
        }

        private void StopHurtAnimation() {
            animator.SetBool(hurtParameter, false);
        }

        private void OnPlayerInvincible() {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = invincibleAlpha;
            spriteRenderer.color = spriteColor;
        }

        private void OnPlayerNotInvincible() {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = initialAlpha;
            spriteRenderer.color = spriteColor;
        }

        private void OnJumpAnticipation() {
            animator.SetBool(anticipateJumpParameter, true);
        }

        private void OnJumpExecuted() {
            animator.SetBool(anticipateJumpParameter, false);
        }

        private void OnPlayerDied() {
            animator.SetBool(dieParameter, true);
        }

        private void OnDeathAnimationFinished() {
            gameManager.EndGameAfterAnimation();
        }
    }
}