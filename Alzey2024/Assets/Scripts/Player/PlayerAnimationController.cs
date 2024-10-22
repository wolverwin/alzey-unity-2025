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
        private float initialAlpha;

        private void Start() {
            initialAlpha = spriteRenderer.color.a;

            EventManager.OnPlayerHurt += TriggerHurtAnimation;
            EventManager.OnPlayerRecovered += StopHurtAnimation;
            EventManager.OnPlayerInvincible += OnPlayerInvincible;
            EventManager.OnPlayerNotInvincible += OnPlayerNotInvincible;
            EventManager.OnPlayerDied += OnPlayerDied;
        }

        private void Update() {
            animator.SetFloat("Speed", Mathf.Abs(body.velocity.x));

            bool isJumping = false;
            bool isFalling = false;

            if (body.velocity.y > PlayerMovementController.MIN_VERTICAL_VELOCITY) {
                isJumping = true;
            } else if (body.velocity.y < -PlayerMovementController.MIN_VERTICAL_VELOCITY) {
                isFalling = true;
            }

            animator.SetBool("Jumping", isJumping);
            animator.SetBool("Falling", isFalling);
        }

        private void TriggerHurtAnimation(Vector3 source) {
            animator.SetBool("Hurt", true);
        }

        private void StopHurtAnimation() {
            animator.SetBool("Hurt", false);
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

        private void OnPlayerDied() {
            animator.SetBool("Die", true);
        }
    }
}
