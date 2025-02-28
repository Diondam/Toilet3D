using System;
using Unicorn.Unicorn.Scripts.Controller.SheepGame;
using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Character.Sheep
{
    public class PlayerJump : MonoBehaviour
    {
        public float jumpForce = 5.0f; // Lực nhảy, càng cao nhân vật càng nhảy cao
        public float forwardForce = 2.0f; // Lực tiến về phía trước
        private Rigidbody rb; // Tham chiếu đến Rigidbody của nhân vật

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // 0 là nút chuột trái
            {
                if (IsGrounded())
                {
                    rb.AddForce(new Vector3(0, jumpForce, forwardForce), ForceMode.Impulse);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("WinPlace"))
            {
               SheepLevelManager.Instance.Win();
            }
            if (other.gameObject.CompareTag("Finish"))
            {
                SheepLevelManager.Instance.End();
            }
        }


        private void OnCollisionEnter(Collision other)
        {
           
            if (other.gameObject.CompareTag("Buoy"))
            {
                other.gameObject.GetComponent<Wolf>().enabled = false;
            }
          
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Buoy"))
            {
                other.gameObject.GetComponent<Wolf>().enabled = true;

            }
        }

        private bool IsGrounded()
        {
            // Kiểm tra xem nhân vật có đang tiếp đất hay không
            return Physics.Raycast(transform.position, -Vector3.up, 1.11f);
        }
    }
}