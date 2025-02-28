using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputHandler : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the movement speed as needed
    private void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            // Get the first touch (you can modify this for multitouch support)
            Touch touch = Input.GetTouch(0);

            // Calculate the movement direction based on touch position
            Vector3 moveDirection = Vector3.zero;

            if (touch.position.x < Screen.width * 0.5f)
            {
                // Touch on the left half of the screen: move left
                moveDirection = Vector3.left;
            }
            else
            {
                // Touch on the right half of the screen: move right
                moveDirection = Vector3.right;
            }

            // Apply the movement to the player
            MovePlayer(moveDirection.normalized);
        }
    }

    private void MovePlayer(Vector3 direction)
    {
        // Move the player in the specified direction
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
