using Manager;
using UnityEngine;

namespace Player {
    public class PlayerFinishController : MonoBehaviour {
        private GameManager gameManager;

        private void Start() {
            gameManager = GameManager.Instance;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (!collision.CompareTag("Finish")) {
                return;
            }

            gameManager.FinishLevel();
        }
    }
}