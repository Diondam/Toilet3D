using System;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using Unicorn.Unicorn.Scripts.Controller.DuckGame;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CharacterPlayer : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 8.0f;
    public float gravity = 20.0f;
    private CharacterController controller;
    private Vector3 moveDirection;
    public Slider HPCharacter;
    public Transform handPos;
    public Animator anim;
    public GameObject duckOnHand;
    public bool isCatched;

    private void Start()
    {
       controller = GetComponent<CharacterController>();
       anim.SetTrigger("IdleAni");

        // if (GameManager.Instance.CurrentLevel >= 3)
        // {
        //     HPCharacter.gameObject.SetActive(true);
        // }
    }


    private void OnTriggerEnter(Collider other)
    {
        print("vacham");
        if (other.gameObject.CompareTag("PlaceAnimal")) //&& có mèo trên tay
        {
            print("va cham PlaceAnimal");
            // thực hiện anim đặt
            if (duckOnHand != null)
            {
                //thả xuống
                duckOnHand.transform.parent = DuckLevelManager.Instance.transform;
                anim.SetTrigger("IdleAni");
                //cho chạy quanh
                duckOnHand.GetComponent<NavMeshAgent>().enabled = true;
                duckOnHand.GetComponent<NavMeshAgent>().SetDestination(other.transform.position);
                if (duckOnHand.GetComponent<Duck>().isDead)
                {
                    int enemy = --DuckLevelManager.Instance.numberNeedToWin;
                    if (enemy == 0) DuckLevelManager.Instance.Win();
                }
            }
            duckOnHand = null;
            isCatched = false;
        }
    }

    private void Update()
    {
        //print("CHAR DUCK: "+ transform.position);
        //HPCharacter.value -= Time.deltaTime * 0.01f;
        if (controller.isGrounded)
        {
            // Get input from the player
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calculate the move direction
            moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput);
            moveDirection = transform.TransformDirection(moveDirection); // Convert to world space

            // Apply move speed
            moveDirection *= moveSpeed;

            // JumpController
            // if (Input.GetButtonDown("JumpController"))
            // {
            //     moveDirection.y = jumpForce;
            // }
        }
        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;
        // Move the character
        controller.Move(moveDirection * Time.deltaTime);
    }
}