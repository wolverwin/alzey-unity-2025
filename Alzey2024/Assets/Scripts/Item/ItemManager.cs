using UnityEngine;

namespace Manager {
    public class ItemManager : MonoBehaviour {
        private void Start() {
            GameManager.Instance.ItemCount = transform.childCount;
        }
    }
}

