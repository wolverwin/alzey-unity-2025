using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager {
    public class GameManager : MonoBehaviour {

        private static GameManager instance;

        public static GameManager Instance {
            get {
                if (instance == null) {
                    Debug.LogError("GameManager is null!");
                }

                return instance;
            } 
        }

        private UIManager uiManager;

        /// <summary>
        /// The base health of the player
        /// </summary>
        [SerializeField]
        private int baseHealth = 3;

        public int BaseHealth {
            get {
                return baseHealth;
            }
        }

        /// <summary>
        /// The current damage of the player
        /// </summary>
        private int damage;

        public int Damage {
            get {
                return damage;
            }
            set {
                damage = value;
                uiManager?.UpdateUI();

                if (damage >= baseHealth) { 
                    EndGame();
                }
            }
        }

        /// <summary>
        /// The count of items collected by the player
        /// </summary>
        private int itemsCollected;

        public int ItemsCollected {
            get {
                return itemsCollected;
            }

            set { 
                itemsCollected = value;
                uiManager?.UpdateUI();

                if (itemsCollected >= itemCount) {
                    EventManager.InvokeOnRequirementsFulfilled();
                }
            }
        }

        /// <summary>
        /// How many items are in this level
        /// </summary>
        private int itemCount;

        public int ItemCount {
            get {
                return itemCount;
            }

            set {
                itemCount = value;
            }
        }

        /// <summary>
        /// Whether the game is paused or not
        /// </summary>
        private bool gamePaused;

        public bool GamePaused {
            get {
                return gamePaused; 
            } 
        }

        private void Awake() {
            instance = this;
        }

        private void Start() {
            EventManager.Reset();
            uiManager = UIManager.Instance;

            Time.timeScale = 1;
        }

        private void Update() {
            if (Input.GetButtonDown("Cancel")) {
                gamePaused = !gamePaused;
                uiManager?.TogglePauseMenu();

                if (gamePaused) {
                    Time.timeScale = 0;
                } else {
                    Time.timeScale = 1;
                }
            }
        }

        /// <summary>
        /// Reloads the current scene
        /// </summary>
        public void Reload() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Restarts the game from the first level
        /// </summary>
        public void Restart() {
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Exits the game
        /// </summary>
        public void Exit() {
            Application.Quit();
        }

        /// <summary>
        /// Finishes the level and either shows the end screen or starts the next level
        /// </summary>
        public void FinishLevel() {
            int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

            // No next level, end the game here
            if (nextLevelIndex >= SceneManager.sceneCountInBuildSettings) {
                Time.timeScale = 0;
                uiManager?.ShowWinScreen();
                return;
            }

            SceneManager.LoadScene(nextLevelIndex);
        }

        /// <summary>
        /// Ends the game
        /// </summary>
        private void EndGame() {
            Time.timeScale = 0;
            uiManager?.ShowLoseScreen();
        }
    }
}

