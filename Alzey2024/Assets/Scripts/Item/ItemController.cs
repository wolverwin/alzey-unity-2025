using UnityEngine;

namespace Item {
    public class ItemController : MonoBehaviour {

        [SerializeField]
        ItemAnimationController animationController;

        bool collected;

        public bool Collected {
            get { return collected; } 
        }

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

