using UnityEngine;

namespace Misc {
    public class ParallaxController : MonoBehaviour {
        /// <summary>
        /// How fast the background moves relative to the camera. 0 = moves with cam || 1 won't move
        /// </summary>
        [SerializeField, Range(0, 1), Tooltip("0 moves with cam || 1 won't move")]
        private float modifier;

        private Camera cam;
        private float startPositionX;
        private float startPositionY;
        private float width;

        private void Start() {
            width = GetComponent<SpriteRenderer>().bounds.size.x;
            cam = Camera.main;
            startPositionX = transform.position.x;
            startPositionY = transform.position.y;
        }

        private void FixedUpdate() {
            // Calculate distance background move base on cam movement
            float distanceX = cam.transform.position.x * modifier;
            float distanceY = cam.transform.position.y * modifier;
            float movement = cam.transform.position.x * (1 - modifier);

            transform.position = new Vector3(startPositionX + distanceX, startPositionY + distanceY, transform.position.z);

            // If background has reached the end of its length, adjust position for infinite scrolling
            if (movement > startPositionX + width) {
                startPositionX += width;
            }

            if (movement < startPositionX - width) {
                startPositionX -= width;
            }
        }
    }
}