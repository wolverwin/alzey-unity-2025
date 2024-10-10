using UnityEngine;

namespace Manager {
    public class ItemManager : MonoBehaviour {
        void Start() {
            GameManager.Instance.ItemCount = transform.childCount;
        }
    }
}

