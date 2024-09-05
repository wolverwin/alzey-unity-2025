using UnityEngine;

public class AnimationController : MonoBehaviour {

    [SerializeField]
    Animator animator;

    public void UpdateMovement(float horizontalMovement, float velocityY, bool isClimbing) {
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
        animator.SetBool("Climbing", isClimbing);

        if (velocityY > 0.1f) {
            animator.SetBool("Jumping", true);
            animator.SetBool("Falling", false);
        } else if (velocityY < -0.1f) {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", true);
        } else {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
        }
    }

    public void SetHurt(bool isHurt) {
        animator.Play("Hurt");
    }
}
