using Unicorn.Unicorn.Scripts.Controller.FlyGame;
using Unicorn.Unicorn.Scripts.Controller.JumpGame;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.Unicorn.Scripts.Character.Fly
{
    public class FlyController : MonoBehaviour
    {
        public float acceleration = 5.0f; // Tăng tốc
        public float maxSpeed = 10.0f; // Tốc độ tối đa
        private Rigidbody rb; // Rigidbody của xe để áp dụng lực
        private float currentSpeed; // Tốc độ hiện tại của xe
        FlyLevelManager LM => FlyLevelManager.Instance;
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            // Di chuyển với tốc độ trung bình
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
            }
            else if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }

            TurnCar(swipeDelta.x);
            if (transform.position.y < -24)
            {
                LM.End();
            }
            rb.velocity = transform.rotation * Vector3.zero +
                          (transform.forward) * currentSpeed;
        }

        public float fixPos = 5.7f;

        public float turnSpeed = 5f; // Tốc độ quay của xe
        private Vector2 startTouchPosition;
        private Vector2 swipeDelta;
        private bool isSwiping = false;
        private float currentTurnAngle = 0f;
        public float maxTurnAngle = 20f; // Giới hạn góc quay tối đa

        void Update()
        {
            GetSwipeInput();
        }

        private bool isAddForce;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Finish"))
            {
                print("JUMP WIN");
                JumpLevelManager.Instance.Win();
                isAddForce = true;
            }

            if (!isAddForce)
            {
                isAddForce = true;
                if (other.gameObject.CompareTag("Wall"))
                {
                    rb.AddForce(transform.up * amountForce, ForceMode.Impulse);
                }
            }
        }

        public float amountForce = 50f;


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
            transform.localEulerAngles = new Vector3(0f, currentTurnAngle, 0f);
        }
    }
}