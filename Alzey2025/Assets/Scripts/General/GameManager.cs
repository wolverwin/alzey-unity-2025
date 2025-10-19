using UnityEngine;
using UnityEngine.InputSystem;
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

        public int BaseHealth => baseHealth;

        public int CurrentHealth => baseHealth - damage;

        [SerializeField, Tooltip("Ends the game after the death animation has played")]
        private bool endGameAfterAnimation;

        /// <summary>
        /// The current damage of the player
        /// </summary>
        private int damage;

        public int Damage {
            get => damage;
            set {
                damage = value;
                uiManager?.UpdateUI();

                if (damage >= baseHealth) {
                    EventManager.InvokeOnPlayerDied();
                }
            }
        }

        /// <summary>
        /// The count of items collected by the player
        /// </summary>
        private int itemsCollected;

        public int ItemsCollected {
            get => itemsCollected;

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
            get => itemCount;

            set => itemCount = value;
        }

        /// <summary>
        /// Whether the game is paused or not
        /// </summary>
        private bool gamePaused;

        public bool GamePaused => gamePaused;

        public bool IsPlayerDead => damage >= baseHealth;

        private InputAction pauseAction;

        private void Awake() {
            instance = this;
        }

        private void Start() {
            EventManager.Reset();
            uiManager = UIManager.Instance;
            pauseAction = InputSystem.actions.FindAction("Cancel");

            EventManager.OnPlayerDied += EndGame;

            Time.timeScale = 1;
        }

        private void Update() {
            if (!pauseAction.WasPressedThisFrame()) {
                return;
            }

            gamePaused = !gamePaused;
            uiManager?.TogglePauseMenu();

            Time.timeScale = gamePaused ? 0 : 1;
        }

        /// <summary>
        /// Reloads the current scene
        /// </summary>
        public static void Reload() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Restarts the game from the first level
        /// </summary>
        public static void Restart() {
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Exits the game
        /// </summary>
        public static void Exit() {
            Application.Quit();
            Debug.Log("Triggered application quit!");
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
        /// Ends the game after an animation has played
        /// </summary>
        public void EndGameAfterAnimation() {
            Time.timeScale = 0;
            uiManager?.ShowLoseScreen();
        }

        /// <summary>
        /// Ends the game
        /// </summary>
        private void EndGame() {
            if (endGameAfterAnimation) {
                return;
            }

            Time.timeScale = 0;
            uiManager?.ShowLoseScreen();
        }
    }
}