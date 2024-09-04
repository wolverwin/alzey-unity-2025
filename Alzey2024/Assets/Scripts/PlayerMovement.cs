using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 40;

    [SerializeField]
    CharacterController2D characterController;

    float horizontalMovement = 0;

    bool jump;

    // Update is called once per frame
    void Update() {
        horizontalMovement = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }
    }

    void FixedUpdate() {
        characterController.Move(horizontalMovement * Time.fixedDeltaTime, jump, false, 0);
        jump = false;
    }
}
