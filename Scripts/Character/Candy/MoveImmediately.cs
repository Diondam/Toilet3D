using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unicorn
{
    public class MoveImmediately : MonoBehaviour
    {
        [BoxGroup("Properties")]
        [SerializeField] protected float speed = 10;
        [BoxGroup("Properties")]
        [SerializeField] protected float rotationSpeed = 180;
        [BoxGroup("Properties")]
        [SerializeField] protected Vector3 gravity = new Vector3(0, -9.81f, 0);

        [SerializeField] protected CharacterController characterController;
        public bool CanMove = true;
        public float Speed => speed;
        private UltimateJoystick ultimateJoystick;
        private Quaternion currentRotation;
        private Vector3 outerMotion;

        [SerializeField] private bool invert;
        public UltimateJoystick Joystick;

        private Animator anim;
        public Vector3 OuterMotion
        {
            get => outerMotion;
            private set => outerMotion = value;
        }

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            if (!characterController)
            {
                characterController = GetComponent<CharacterController>();
            }
            currentRotation = transform.rotation;

        }
        void Update()
        {
            OnUpdate();
        }
      
        private Vector3 lastMousePosition;
        private Vector3 joystickMotion = Vector3.zero;
        public void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 currentMousePosition = Input.mousePosition;
                joystickMotion = currentMousePosition - lastMousePosition;
                lastMousePosition = currentMousePosition;
                if (joystickMotion.x < 0)
                {
                    joystickMotion= Vector3.left;
                }
                else if (joystickMotion.x > 0)
                {
                    joystickMotion= Vector3.right;
                }
            }

            if (invert)
            {
                joystickMotion *= -1;
            }

            if (joystickMotion != Vector3.zero)
            {
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Run", false);
            }
            
            //UpdateMove(joystickMotion * Speed);
            Rotate(joystickMotion);
        }

        private void UpdateMove(Vector3 motion)
        {
            if (!CanMove)
            {
                characterController.Move(OuterMotion * Time.deltaTime);
                OuterMotion = Vector3.zero;
                return;
            }

            Vector3 moveMotion = motion;
            moveMotion += gravity;
            moveMotion += OuterMotion;
            characterController.Move(moveMotion * Time.deltaTime);
            OuterMotion = Vector3.zero;
            //animator.SetBool("Run", true);
        }
        public void Move(Vector3 direction)
        {
            OuterMotion += direction;
           
        }

        private void Rotate(Vector3 joystickMotion)
        {
            if (!CanMove) return;
            if (joystickMotion == Vector3.zero) return;

            var newRotation = Quaternion.LookRotation(joystickMotion, Vector3.up);
            currentRotation = Quaternion.Slerp(
                transform.rotation,
                newRotation,
                1 - Mathf.Exp(-rotationSpeed * Time.deltaTime));
            transform.rotation = currentRotation;
        }
    }
}
