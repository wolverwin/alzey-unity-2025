using UnityEngine;

namespace UI {
    public abstract class UIController : MonoBehaviour {
        /// <summary>
        /// Updates the UI of this controller
        /// </summary>
        public abstract void UpdateUI();
    }
}