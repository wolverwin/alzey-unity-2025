using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILiveController : UIController {

    [SerializeField]
    GameObject live;

    [SerializeField]
    GameObject lostLive;

    GameManager gameManager;
    int liveCount;
    List<GameObject> instantiatedLives;
    int currentDamage;
    HorizontalLayoutGroup horizontalLayoutGroup;

    void Start() {
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
            for (int i = liveCount - 1 - currentDamage; i >= liveCount - newDamage; i--) {
                GameObject instantiatedLive = instantiatedLives[i];

                int liveIndex = instantiatedLive.transform.GetSiblingIndex();// GetGameObjectIndex(transform, instantiatedLife);

                if (liveIndex < 0) {
                    Debug.LogError("GameObject search returned -1. This should not happen!");
                    
                    return;
                }

                Destroy(instantiatedLive);

                GameObject lostLive = Instantiate(this.lostLive, transform);
                lostLive.transform.SetSiblingIndex(liveIndex);
                instantiatedLives[i] = lostLive;
            }
        } else {

        }

        currentDamage = newDamage;
    }

    /// <summary>
    /// Searches the given target as child of the given parent and returns the index of the child
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    int GetGameObjectIndex(Transform parent, GameObject target) {
        int childCount = transform.childCount;
        
        for (int i = 0; i < childCount; i++) {
            if (parent.GetChild(i) == target.gameObject) {
                return i;
            }
        }

        return -1;
    }
}
