using Item;
using Manager;
using UnityEngine;

namespace Player {
    public class PlayerCollectionController : MonoBehaviour {
        private GameManager gameManager;

        private void Start() {
            gameManager = GameManager.Instance;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (!collision.CompareTag("Collectable")) {
                return;
            }

            ItemController itemController = collision.GetComponent<ItemController>();

            if (itemController.Collected) {
                return;
            }

            itemController.OnItemCollected();

            if (gameManager != null) {
                gameManager.ItemsCollected++;
            }
        }
    }
}