using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using Unicorn.Unicorn.Scripts.Controller.DuckGame;
using Unity.Mathematics;

[RequireComponent(typeof(NavMeshAgent))]
public class Ghost : MonoBehaviour
{
    private DuckLevelManager _levelManager = DuckLevelManager.Instance;

    public float destroyTime = 0.4f;
    //co the keo tay
    public NavMeshAgent agent;
    //mặc định mang giá trị false
    private bool isDead;
    public GameObject slashEffectPrefab;

    
    private void Awake()
    {
        _levelManager.numberNeedToWin += 1;
        agent = GetComponent<NavMeshAgent>();
        slashEffectPrefab = Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
        slashEffectPrefab.transform.parent = _levelManager.transform;
    }

    private void Update()
    {
        ForcePosition();
        Transform player = _levelManager.characterDuck.transform;

        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);


        // Check if the distance is less than or equal to the minimum distance
        if (distanceToPlayer <= _levelManager.minDistanceToPlayer)
        {
            // Calculate the direction from the enemy to the player
            Vector3 directionToPlayer = transform.position - player.position;

            // Calculate the desired position away from the player
            Vector3 targetPosition = transform.position + directionToPlayer.normalized * _levelManager.minDistanceToPlayer;

            // Move the enemy to the desired position
            agent.SetDestination(targetPosition);

            // Optionally, adjust the move speed
            agent.speed = _levelManager.duckSpeed;
        }
        transform.LookAt(player);
    }

    void ForcePosition()
    {
        float posX = transform.position.x;
        float posZ = transform.position.z;
        if (posX > 26 || posX < -30)
        {
            transform.position = new Vector3(posX + -posX*0.5f/Mathf.Abs(posX), transform.position.y, posZ);
        }
        if (posZ > 23 || posZ < -32)
        {
            transform.position = new Vector3(posX, transform.position.y,  posZ+ -posZ*0.5f/Mathf.Abs(posZ));
        }
    }


    void DestroyGameObject()
    {
        slashEffectPrefab.transform.position = transform.GetChild(1).position;
        slashEffectPrefab.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isDead)
            {
                isDead = true;
                _levelManager.characterDuck.anim.SetTrigger("Catch");
                Invoke("DestroyGameObject", destroyTime);
                int enemy = --_levelManager.numberNeedToWin;
                if (enemy == 0) _levelManager.Win();
            }
        }
    }

    [Button(ButtonSizes.Medium)]
    private void HaoHao()
    {
        agent = GetComponent<NavMeshAgent>();

    }
}
