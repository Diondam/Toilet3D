using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class MoveDirection : MonoBehaviour
    {
        public float speed = 0.01f; // Tốc độ di chuyển
        private Vector3 lastMousePosition;
        public Animator anim;

        void Update()
        {
            if (transform.position.x >= 3)
            {
                transform.position = new Vector3(3, transform.position.y, transform.position.z);
            }

            if (transform.position.x <= -3)
            {
                transform.position = new Vector3(-3, transform.position.y, transform.position.z);
            }

            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 currentMousePosition = Input.mousePosition;
                Vector3 direction = currentMousePosition - lastMousePosition;
                if (direction == Vector3.zero)
                {
                    anim.SetBool("Run", false);
                }
                if (direction.x < 0)
                {
                    MoveLeft();
                    anim.SetBool("Run", true);
                }
                else if (direction.x > 0)
                {
                    MoveRight();
                    anim.SetBool("Run", true);
                }

                lastMousePosition = currentMousePosition;
                transform.position += new Vector3(direction.x * speed, 0, 0);
            }
        }

        void MoveLeft()
        {
            // Xoay nhân vật về bên trái
            transform.rotation = Quaternion.Euler(0, -90, 0); // -90 độ theo trục Y để quay sang trái
            // Di chuyển nhân vật về bên trái
            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        void MoveRight()
        {
            // Xoay nhân vật về bên phải
            transform.rotation = Quaternion.Euler(0, 90, 0); // 90 độ theo trục Y để quay sang phải
            // Di chuyển nhân vật về bên phải
            //transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }
}