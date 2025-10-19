using UnityEngine;

namespace Item {
    public class ItemController : MonoBehaviour {
        [SerializeField]
        private ItemAnimationController animationController;

        private bool collected;

        public bool Collected => collected;

        /// <summary>
        /// Triggered when player collects this item
        /// </summary>
        public void OnItemCollected() {
            collected = true;
            animationController.StartCollectAnimation();
        }

        /// <summary>
        /// Triggered when the collected animation of this item finished
        /// </summary>
        public void OnItemCollectedAnimationFinished() {
            Destroy(gameObject);
        }
    }
}