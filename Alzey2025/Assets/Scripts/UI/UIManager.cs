using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Manager {
    public class UIManager : MonoBehaviour {
        private static UIManager instance;

        public static UIManager Instance {
            get {
                if (instance == null) {
                    Debug.LogError("UIManager instance is null!");
                }

                return instance;
            }
        }

        [SerializeField]
        private GameObject pauseMenu;

        [SerializeField]
        private GameObject winScreen;

        [SerializeField]
        private GameObject loseScreen;

        private List<UIController> uiController;

        private void Awake() {
            instance = this;
        }

        private void Start() {
            uiController = new List<UIController>(GetComponentsInChildren<UIController>(true));
        }

        public void UpdateUI() {
            for (int i = 0; i < uiController.Count; i++) {
                uiController[i].UpdateUI();
            }
        }

        /// <summary>
        /// Shows the pause menu
        /// </summary>
        public void TogglePauseMenu() {
            if (!pauseMenu) {
                return;
            }

            if (winScreen && loseScreen && (winScreen.activeSelf || loseScreen.activeSelf)) {
                return;
            }

            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        /// <summary>
        /// Shows the win screen to restart the game
        /// </summary>
        public void ShowWinScreen() {
            winScreen?.SetActive(true);
        }

        /// <summary>
        /// Shows the lose screen to restart the game
        /// </summary>
        public void ShowLoseScreen() {
            loseScreen?.SetActive(true);
        }
    }
}