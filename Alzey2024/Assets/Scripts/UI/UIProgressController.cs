using Manager;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UIProgressController : UIController {

        [SerializeField]
        private GameObject progressItem;

        [SerializeField, Tooltip("The sprite to show when the item was collected. Only relevant if animated is false.")]
        private Sprite collectedSprite;

        [SerializeField, Tooltip("If the progress should be animated. If set to true, animationParameter must be set!")]
        private bool animated;

        [SerializeField, Tooltip("The parameter to set for the animation. Only relevant if animated is true.")]

        private string animationParameter = "FillGlass";
        private GameManager gameManager;
        private int itemCount;
        private List<GameObject> instantiatedItems;
        private int currentProgress;

        private void Start() {
            gameManager = GameManager.Instance;
            itemCount = gameManager.ItemCount;
            instantiatedItems = new List<GameObject>();

            for (int i = 0; i < itemCount; i++) {
                instantiatedItems.Add(Instantiate(progressItem, transform));
            }
        }

        public override void UpdateUI() {
            int newProgress = gameManager.ItemsCollected;

            if (newProgress == currentProgress) {
                return;
            }

            for (int i = currentProgress; i < newProgress; i++) {
                if (animated) {
                    Animator animator = instantiatedItems[i].GetComponent<Animator>();
                    animator.SetBool(animationParameter, true);
                } else {
                    Image image = instantiatedItems[i].GetComponent<Image>();
                    image.sprite = collectedSprite;
                }
            }

            currentProgress = newProgress;
        }
    }
}