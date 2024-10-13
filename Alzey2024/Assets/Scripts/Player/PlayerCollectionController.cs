using Item;
using Manager;
using UnityEngine;

namespace Player {
    public class PlayerCollectionController : MonoBehaviour {
        void OnTriggerEnter2D(Collider2D collision) {
            if (collision.tag != "Collectable") {
                return;
            }

            ItemController itemController = collision.GetComponent<ItemController>();

            if (itemController.Collected) {
                return;
            }

            itemController.OnItemCollected();
            GameManager.Instance.ItemsCollected++;
        }
    }
}
