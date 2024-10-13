using Manager;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour {

        [SerializeField]
        Rigidbody2D body;

        [SerializeField]
        SpriteRenderer spriteRenderer;

        [SerializeField]
        Animator animator;

        [SerializeField, Range(0, 1)]
        float invincibleAlpha = 0.5f;

        float initialAlpha;

        void Start() {
            initialAlpha = spriteRenderer.color.a;

            EventManager.OnPlayerHurt += TriggerHurtAnimation;
            EventManager.OnPlayerRecovered += StopHurtAnimation;
            EventManager.OnPlayerInvincible += OnPlayerInvincible;
            EventManager.OnPlayerNotInvincible += OnPlayerNotInvincible;
        }

        void Update() {
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

        void TriggerHurtAnimation() {
            animator.SetBool("Hurt", true);
        }

        void StopHurtAnimation() {
            animator.SetBool("Hurt", false);
        }

        void OnPlayerInvincible() {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = invincibleAlpha;
            spriteRenderer.color = spriteColor;
        }

        void OnPlayerNotInvincible() {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = initialAlpha;
            spriteRenderer.color = spriteColor;
        }
    }
}
