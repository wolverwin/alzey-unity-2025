using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectionController : MonoBehaviour {
    void Start() {
        
    }

    void Update() {
        
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag != "Collectable") {
            return;
        }

        Destroy(collision.gameObject);
    }
}
