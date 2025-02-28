using Unicorn.Unicorn.Scripts.Controller.BoatGame;
using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Character.Boat
{
    public class BoatController : MonoBehaviour
    {
        public Animator anim;
        public float acceleration = 5.0f; // Tăng tốc
        public float maxSpeed = 10.0f; // Tốc độ tối đa
        private Rigidbody rb; // Rigidbody của xe để áp dụng lực
        private float currentSpeed; // Tốc độ hiện tại của xe
        public float stepMove = 1;
        private BoatLevelManager LM => BoatLevelManager.Instance;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            anim.SetBool("Surf", true);
        }

        private void FixedUpdate()
        {
            print("transform.position.y: "+ transform.position.y);
            // Di chuyển với tốc độ trung bình
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
            }
            else if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }

            //if(-fixPos<=transform.position.x&& transform.position.x<= fixPos)
            if (!isWin)
            {
                Move();
            }
        }

        void Move()
        {
            rb.velocity = transform.rotation * Vector3.zero +
                          transform.forward * currentSpeed;
        }

        public float fixPos = 5.7f;

        public float turnSpeed = 5f; // Tốc độ quay của xe
        private Vector2 startTouchPosition;
        private Vector2 swipeDelta;
        private bool isSwiping = false;
        private float currentTurnAngle = 0f;
        public float maxTurnAngle = 20f; // Giới hạn góc quay tối đa
        private bool isWin;
        void Update()
        {
            //GetSwipeInput();
            // print("Instance: " + LM);
            TurnCar(swipeDelta.x);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeft();
                anim.SetBool("Surf", false);
                anim.SetBool("TurnLeft", true);
                anim.SetBool("TurnRight", false);
            }

            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveRight();
                anim.SetBool("Surf", false);
                anim.SetBool("TurnRight", true);
                anim.SetBool("TurnLeft", false);

            }

            if (transform.position.y < -24)
            {
                LM.End();
            }
        }

        private bool isCollide;
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Finish"))
            {
                isWin = true;
                LM.Win();
            }

            if (other.gameObject.CompareTag("WallOver") && !isCollide)
            {
                isCollide = true;
                Invoke("DelayActive", 0.5f);
                //left
                if (isRight == 1)
                {
                    MoveRight();
                    anim.SetBool("TurnRight", true);
                    anim.SetBool("TurnLeft", false);
                }
                //right
                else if (isRight == 2)
                {
                    MoveLeft();
                    anim.SetBool("TurnLeft", true);
                    anim.SetBool("TurnRight", false);
                }
            }
        }

        void DelayActive()
        {
            isCollide = false;
        }

        public float amountForce = 50f;
        private int isRight = 0;

        public void MoveLeft()
        {
            print("manyyyyyyyyy time");
            isRight = 1;
            swipeDelta = Vector2.left;
            // if (transform.position.x > -fixPos)

            transform.position =
                Vector3.MoveTowards(transform.position, transform.position + Vector3.left, Time.deltaTime);
            
            // transform.position += Vector3.left * stepMove;
        }

        public void MoveRight()
        {
            isRight = 2;
            swipeDelta = Vector2.right;
            //if (transform.position.x < fixPos)
          
            transform.position =
                Vector3.MoveTowards(transform.position, transform.position + Vector3.right, Time.deltaTime);
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
            transform.localEulerAngles = new Vector3(0f, currentTurnAngle, 0f);
        }
    }
}