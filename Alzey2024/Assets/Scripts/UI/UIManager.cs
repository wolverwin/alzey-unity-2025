using System.Collections.Generic;
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
        List<UIController> uiController;

        void Awake() {
            instance = this;
        }

        public void UpdateUI() {
            for (int i = 0; i < uiController.Count; i++) {
                uiController[i].UpdateUI();
            }
        }
    }
}
