using Item;
using Manager;
using UnityEngine;

namespace Player {
    public class PlayerCollectionController : MonoBehaviour {
        
        GameManager gameManager;

        void Start() {
            gameManager = GameManager.Instance;    
        }

        void OnTriggerEnter2D(Collider2D collision) {
            if (collision.tag != "Collectable") {
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
