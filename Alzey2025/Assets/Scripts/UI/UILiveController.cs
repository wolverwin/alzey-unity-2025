using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UILiveController : UIController {

        [SerializeField]
        private GameObject live;

        [SerializeField]
        private Sprite lostLiveSprite;

        [SerializeField, Tooltip("If the progress should be animated. If set to true, animationParameter must be set!")]
        private bool animated;

        [SerializeField, Tooltip("The parameter to set for the animation. Only relevant if animated is true.")]
        private string animationParameter = "LostLive";

        private GameManager gameManager;
        private int liveCount;
        private List<GameObject> instantiatedLives;
        private int currentDamage;
        private HorizontalLayoutGroup horizontalLayoutGroup;

        private void Start() {
            gameManager = GameManager.Instance;
            liveCount = gameManager.BaseHealth;
            currentDamage = 0;
            horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();

            instantiatedLives = new List<GameObject>();

            for (int i = 0; i < liveCount; i++) {
                instantiatedLives.Add(Instantiate(live, transform));
            }
        }

        public override void UpdateUI() {
            int newDamage = gameManager.Damage;
            int damageDelta = newDamage - currentDamage;

            if (damageDelta == 0) {
                return;
            }

            if (damageDelta > 0) {
                for (int i = liveCount - 1 - currentDamage; i >= liveCount - newDamage && i >= 0; i--) {
                    GameObject instantiatedLive = instantiatedLives[i];

                    int liveIndex = instantiatedLive.transform.GetSiblingIndex();// GetGameObjectIndex(transform, instantiatedLife);

                    if (liveIndex < 0) {
                        Debug.LogError("GameObject search returned -1. This should not happen!");

                        return;
                    }

                    if (animated) {
                        Animator animator = instantiatedLive.GetComponent<Animator>();
                        animator.SetBool(animationParameter, true);
                    } else {
                        Image image = instantiatedLive.GetComponent<Image>();
                        image.sprite = lostLiveSprite;
                    }

                    /*
                    Destroy(instantiatedLive);

                    GameObject lostLive = Instantiate(this.lostLive, transform);
                    lostLive.transform.SetSiblingIndex(liveIndex);
                    instantiatedLives[i] = lostLive;
                    */
                }
            }

            currentDamage = newDamage;
        }

        /// <summary>
        /// Searches the given target as child of the given parent and returns the index of the child
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private int GetGameObjectIndex(Transform parent, GameObject target) {
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++) {
                if (parent.GetChild(i) == target.gameObject) {
                    return i;
                }
            }

            return -1;
        }
    }
}
