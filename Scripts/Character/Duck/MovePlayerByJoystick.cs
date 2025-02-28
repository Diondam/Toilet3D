using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unicorn
{
    public class MovePlayerByJoystick : MonoBehaviour
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
        private CharacterPlayer chara;
        public Vector3 OuterMotion
        {
            get => outerMotion;
            private set => outerMotion = value;
        }

        // Start is called before the first frame update
        void Start()
        {
            chara = GetComponent<CharacterPlayer>();
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

        public void OnUpdate()
        {
            Vector3 joystickMotion = new Vector3(
                    Joystick.GetHorizontalAxis(), 0,
                    Joystick.GetVerticalAxis())
                .normalized;

            if (invert)
            {
                joystickMotion *= -1;
            }

            if (joystickMotion != Vector3.zero)
            {
                anim.SetBool("RunCat", true);
                if (chara.duckOnHand != null)
                {
                    anim.SetBool("RunHug", true);
                }
            }
            else
            {
                anim.SetBool("RunCat", false);
                if (chara.duckOnHand != null)
                {
                    anim.SetBool("RunHug", false);
                }
            }
            
            UpdateMove(joystickMotion * Speed);
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
