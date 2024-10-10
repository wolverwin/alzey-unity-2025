using UnityEngine;

namespace Manager {
    public class GameManager : MonoBehaviour {
        
        static GameManager instance;

        public static GameManager Instance {
            get {
                if (instance == null) {
                    Debug.LogError("GameManager is null!");
                }

                return instance;
            } 
        }

        UIManager uiManager;

        /// <summary>
        /// The count of items collected by the player
        /// </summary>
        int itemsCollected;

        public int ItemsCollected {
            get {
                return itemsCollected;
            }

            set { 
                itemsCollected = value;
                uiManager.UpdateUI();
            }
        }

        /// <summary>
        /// How many items are in this level
        /// </summary>
        int itemCount;

        public int ItemCount {
            get {
                return itemCount;
            }

            set {
                itemCount = value;
            }
        }

        void Awake() {
            instance = this;
        }

        void Start() {
            uiManager = UIManager.Instance;
        }
    }
}

