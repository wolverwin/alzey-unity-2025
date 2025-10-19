using UnityEngine;

namespace Item {
    public class ItemAnimationController : MonoBehaviour {
        [SerializeField]
        private ItemController controller;

        [SerializeField]
        private Animator animator;

        /// <summary>
        /// Triggered when the collected animation finished
        /// </summary>
        private void CollectAnimationFinished() {
            controller.OnItemCollectedAnimationFinished();
        }

        /// <summary>
        /// Starts the collected animation
        /// </summary>
        public void StartCollectAnimation() {
            animator.SetBool("Collected", true);
        }
    }
}