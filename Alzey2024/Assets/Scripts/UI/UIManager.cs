using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Manager {
    public class UIManager : MonoBehaviour {

        static UIManager instance;

        public static UIManager Instance {
            get {
                if (instance == null) {
                    Debug.LogError("UIManager instance is null!");
                }

                return instance;
            }
        }

        [SerializeField]
        GameObject pauseMenu;

        [SerializeField]
        GameObject endScreen;

        List<UIController> uiController;

        void Awake() {
            instance = this;
        }

        void Start() {
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
            if (endScreen.activeSelf) {
                return;
            }

            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        /// <summary>
        /// Shows the end screen to restart the game
        /// </summary>
        public void ShowEndScreen() {
            endScreen.SetActive(true);
        }
    }
}
