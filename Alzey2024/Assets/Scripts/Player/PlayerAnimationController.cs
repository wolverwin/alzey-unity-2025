using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour {

        [SerializeField]
        Rigidbody2D body;

        [SerializeField]
        Animator animator;

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
    }
}
