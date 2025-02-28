using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightLeftMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 8.0f;
    public float gravity = 20.0f;

    private CharacterController controller;
    private Vector3 moveDirection;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {

        if (controller.isGrounded)
        {
            // Get input from the player
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calculate the move direction
            moveDirection = new Vector3(horizontalInput, 0.0f, 0);
            moveDirection = transform.TransformDirection(moveDirection); // Convert to world space

            // Apply move speed
            moveDirection *= moveSpeed;

            // JumpController
            if (Input.GetButtonDown("JumpController"))
            {
                moveDirection.y = jumpForce;
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the character
        controller.Move(moveDirection * Time.deltaTime);
        
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

    }
}