using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class SwipeMoveCharacter : MonoBehaviour
    {
        private Vector3 lastDragPosition;

        public float moveSpeed = 0.01f; // Adjust as needed for your desired sensitivity

        void Update()
        {
            //HandleTouchMove();
            HandleMouseDragMove();
        }

        void HandleTouchMove()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 currentTouchPosition = new Vector3(touch.position.x, touch.position.y, 0);

                if (touch.phase == TouchPhase.Moved)
                {
                    Vector3 moveDirection = currentTouchPosition - lastDragPosition;
                    transform.position += new Vector3(moveDirection.x * moveSpeed, 0, 0);
                }

                lastDragPosition = currentTouchPosition;
            }
        }

        public float litmitSwipe;

        void HandleMouseDragMove()
        {
            Vector3 currentMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

            if (Input.GetMouseButton(0))
            {
                Vector3 moveDirection = currentMousePosition - lastDragPosition;
                transform.position += new Vector3(moveDirection.x * moveSpeed, 0, 0);
                if (!(transform.position.x <= litmitSwipe && transform.position.x >= -litmitSwipe))
                {
                    if (transform.position.x > 0)
                    {
                        transform.position = new Vector3((litmitSwipe), 0, 0);
                    }
                    else
                    {
                        transform.position = new Vector3(-(litmitSwipe), 0, 0);
                    }
                }
            }

            lastDragPosition = currentMousePosition;
        }
    }
}