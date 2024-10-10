using Manager;
using System.Collections.Generic;
using UnityEngine;

public class UIProgressController : UIController {

    [SerializeField]
    GameObject progressItem;

    GameManager gameManager;
    int itemCount;
    List<GameObject> instantiatedItems;
    int currentProgress;

    void Start() {
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
            Animator animator = instantiatedItems[i].GetComponent<Animator>();
            animator.SetBool("FillGlass", true);
        }

        currentProgress = newProgress;
    }
}
