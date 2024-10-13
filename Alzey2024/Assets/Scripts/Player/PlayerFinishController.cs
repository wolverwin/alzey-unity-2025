using Manager;
using UnityEngine;

namespace Player {
    public class PlayerFinishController : MonoBehaviour {

        GameManager gameManager;

        void Start() {
            gameManager = GameManager.Instance;
        }

        void OnTriggerEnter2D(Collider2D collision) {
            if (collision.tag != "Finish") {
                return;
            }

            gameManager.FinishLevel();
        }
    }
}
