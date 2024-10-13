using UnityEngine;

namespace Misc {
    public class ParallaxController : MonoBehaviour {

        /// <summary>
        /// How fast the background moves relative to the camera. 0 = moves with cam || 1 won't move
        /// </summary>
        [SerializeField, Range(0, 1)]
        float modifier;

        Camera cam;
        float startPosition;
        float width;

        void Start() {
            width = GetComponent<SpriteRenderer>().bounds.size.x;
            cam = Camera.main;
            startPosition = transform.position.x;
        }

        void FixedUpdate() {
            // Calculate distance background move base on cam movement
            float distance = cam.transform.position.x * modifier;
            float movement = cam.transform.position.x * (1 - modifier);


            transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

            // If background has reached the end of its length, adjust position for infinite scrolling
            if (movement > startPosition + width) {
                startPosition += width;
            }

            if (movement < startPosition - width) {
                startPosition -= width;
            }
        }
    }
}