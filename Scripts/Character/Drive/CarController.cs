using System;
using Unicorn.Unicorn.Scripts.Controller.DriveGame;
using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Character.Drive
{
    public class CarController : MonoBehaviour
    {
        public float acceleration = 5.0f; // Tăng tốc
        public float maxSpeed = 10.0f; // Tốc độ tối đa
        private Rigidbody rb; // Rigidbody của xe để áp dụng lực
        private float currentSpeed; // Tốc độ hiện tại của xe

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private int countFUpdate = 0;
        private void FixedUpdate()
        {
            // countFUpdate++;
            // print("countFUpdate; "+countFUpdate);

            // Di chuyển với tốc độ trung bình
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
            }
            else if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }

            rb.MovePosition(transform.position + transform.rotation * Vector3.zero +
                            transform.forward * currentSpeed * Time.fixedDeltaTime);
        }

        public float turnSpeed = 5f; // Tốc độ quay của xe
        private Vector2 startTouchPosition;
        private Vector2 swipeDelta;
        private bool isSwiping = false;
        private float currentTurnAngle = 0f;
        public float maxTurnAngle = 20f; // Giới hạn góc quay tối đa

        // currentRotation.x = Mathf.Clamp(currentRotation.x, -20, 360);
        //print("currentRotation.x: "+ currentRotation.x);
        public float maxRotationX = 250f;


        private int countUTe = 0;
        void Update()
        {
            // countUTe++;
            // print("countUTe: "+ countUTe);
            Vector3 currentRotation = transform.eulerAngles;
            transform.eulerAngles = currentRotation;

            GetSwipeInput();
            if (isSwiping)
            {
                TurnCar(swipeDelta.x);
            }

            if (transform.position.y < -24)
            {
                DriveLevelManager.Instance.End();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Finish"))
            {
                DriveLevelManager.Instance.Win();
            }
        }

        private void GetSwipeInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPosition = Input.mousePosition;
                isSwiping = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isSwiping = false;
                swipeDelta = Vector2.zero;
            }

            if (isSwiping && Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouchPosition;
            }
        }

        private void TurnCar(float swipeAmount)
        {
            float turnAmount = swipeAmount * turnSpeed * Time.deltaTime;
            // Điều chỉnh góc quay dựa trên vị trí hiện tại và giới hạn góc quay
            currentTurnAngle += turnAmount;
            currentTurnAngle = Mathf.Clamp(currentTurnAngle, -maxTurnAngle, maxTurnAngle);
            // Áp dụng góc quay mới cho xe, nhưng chỉ dọc theo trục y để tránh xoay xe
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentTurnAngle, 0f);
        }
    }
}