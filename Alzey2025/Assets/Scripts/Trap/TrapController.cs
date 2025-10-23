using UnityEngine;

namespace Trap {
    public class TrapController : MonoBehaviour {
        [SerializeField]
        private Animator animator;

        [SerializeField, Tooltip("The damage this trap inflicts")]
        private int damage = 1;

        public int Damage => damage;

        /// <summary>
        /// Triggers the hit animation
        /// </summary>
        public void TriggerHitAnimation() {
            if (animator == null) {
                return;
            }

            animator.SetTrigger("Hit");
        }

        /// <summary>
        /// Destroys this trap
        /// </summary>
        public void DestroyTrap() {
            Destroy(gameObject);
        }
    }
}