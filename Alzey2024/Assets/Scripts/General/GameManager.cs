using System;
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
        /// The base health of the player
        /// </summary>
        [SerializeField]
        int baseHealth = 3;

        public int BaseHealth {
            get {
                return baseHealth;
            }
        }

        /// <summary>
        /// The current damage of the player
        /// </summary>
        int damage;

        public int Damage {
            get {
                return damage;
            }
            set {
                damage = value;
            }
        }

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

