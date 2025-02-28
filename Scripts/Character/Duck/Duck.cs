using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using Unicorn;
using Unicorn.Unicorn.Scripts.Controller.DuckGame;

[RequireComponent(typeof(NavMeshAgent))]
public class Duck : MonoBehaviour
{
    private DuckLevelManager _levelManager = DuckLevelManager.Instance;
    public float destroyTime = 0.4f;
    public NavMeshAgent agent; //co the keo tay

    public bool isDead; //mặc định mang giá trị false

    //public GameObject slashEffectPrefab;
    public Animator anim;
    private bool canMove = true;

    private void Awake()
    {
        _levelManager.numberNeedToWin += 1;
        agent = GetComponent<NavMeshAgent>();
        // slashEffectPrefab = Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
        // slashEffectPrefab.transform.parent = _levelManager.transform;
        anim = GetComponent<Animator>();
    }

    private Transform player;
    private Transform handPos;
    public float magnitude;

    private void Update()
    {
        handPos = _levelManager.characterDuck.handPos;
        //ForcePosition();
        player = _levelManager.characterDuck.transform;
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the distance is less than or equal to the minimum distance
        if (distanceToPlayer <= _levelManager.minDistanceToPlayer)
        {
            // Calculate the direction from the enemy to the player
            Vector3 directionToPlayer = transform.position - player.position;

            // Calculate the desired position away from the player
            Vector3 targetPosition =
                transform.position + directionToPlayer.normalized * _levelManager.minDistanceToPlayer;

            // Move the enemy to the desired position
            if (agent.enabled && !isDead) agent.SetDestination(targetPosition);
            magnitude += Time.deltaTime;

            // Optionally, adjust the move speed
            agent.speed = _levelManager.duckSpeed;
            if (magnitude >= 1) magnitude = 1;
            anim.SetFloat("BlendCat", magnitude);
        }
        else
        {
            if (magnitude >= 0.3) magnitude -= Time.deltaTime * 0.9f;
            magnitude -= Time.deltaTime * 0.09f;
            if (magnitude <= 0) magnitude = 0;
            anim.SetFloat("BlendCat", magnitude);
        }
    }

    void ForcePosition()
    {
        float posX = transform.position.x;
        float posZ = transform.position.z;
        if (posX > 26 || posX < -30 || posZ > 23 || posZ < -32.5f)
        {
            transform.position =
                new Vector3(posX + -posX * 0.5f / posX, transform.position.y, posZ + -posZ * 0.5f / posZ);
        }
    }


    void DestroyGameObject()
    {
        // slashEffectPrefab.transform.position = transform.GetChild(1).position;
        // slashEffectPrefab.GetComponent<ParticleSystem>().Play();
        transform.parent = handPos;
        transform.position = Vector3.zero;
        //Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        handPos = _levelManager.characterDuck.handPos;
        if (other.gameObject.CompareTag("Player") && !isDead)
        {
            if (_levelManager.characterDuck.duckOnHand == null &&  !_levelManager.characterDuck.isCatched)
            {
                _levelManager.characterDuck.isCatched = true;
                isDead = true;
                agent.enabled = false;
                _levelManager.characterDuck.duckOnHand = gameObject;
                _levelManager.characterDuck.anim.SetTrigger("Catch");
                _levelManager.characterDuck.anim.SetTrigger("IdleHug");
              
                var joystick = _levelManager.characterDuck.GetComponent<MovePlayerByJoystick>();
                joystick.CanMove = false;
                Invoke("DelayOnHand", 0.4f);
            }
        }
    }

    void DelayOnHand()
    {
        
        var joystick = _levelManager.characterDuck.GetComponent<MovePlayerByJoystick>();
        joystick.CanMove = true;
        transform.parent = handPos;
        transform.localPosition = Vector3.zero;
    }
}