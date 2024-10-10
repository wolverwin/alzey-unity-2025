using UnityEngine;

namespace Item {
    public class ItemAnimationController : MonoBehaviour {

        [SerializeField]
        ItemController controller;

        [SerializeField]
        Animator animator;

        /// <summary>
        /// Triggered when the collected animation finished
        /// </summary>
        void CollectAnimationFinished() {
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