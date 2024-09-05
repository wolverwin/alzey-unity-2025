using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 40;

    [SerializeField]
    CharacterController2D characterController;

    [SerializeField]
    AnimationController animationController;

    Rigidbody2D body;

    float horizontalMovement = 0;

    bool jump;

    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        horizontalMovement = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }

        animationController.UpdateMovement(horizontalMovement, characterController.IsGrounded ? 0 : body.velocity.y, false);
    }

    void FixedUpdate() {
        characterController.Move(horizontalMovement * Time.fixedDeltaTime, jump, false, 0);
        jump = false;
    }
}
