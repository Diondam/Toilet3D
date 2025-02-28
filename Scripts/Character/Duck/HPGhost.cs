using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using Unicorn.Unicorn.Scripts.Controller.DuckGame;
using UnityEngine.UI;


[RequireComponent(typeof(NavMeshAgent))]
public class HPGhost : MonoBehaviour
{
    private DuckLevelManager _levelManager = DuckLevelManager.Instance;
    public int ghostHP;
    public float destroyTime = 0.4f;
    public NavMeshAgent agent;
    private bool isDead;
    public GameObject slashEffectPrefab;
    public Slider ghostHPSlider;
    private int GhostHPOrin;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _levelManager.numberNeedToWin += 1;
    }

    private void Start()
    {
        GhostHPOrin = ghostHP;
    }

    private void Update()
    {
        InvokeRepeating("AbleToAttack", 0, 0.5f);
        ForcePosition();
        MoveToPlayer();
        if (ghostHPSlider.value == 0 || ghostHP ==0)
        {
            Invoke("DestroyGameObject", destroyTime);
        }
    }

    void AbleToAttack()
    {
        isDead = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isDead)
            {
                isDead = true;
                _levelManager.characterDuck.anim.SetTrigger("Attack");
                Invoke("DestroyGameObject", 0.4f);
                 --_levelManager.numberNeedToWin;
                ghostHP--;
                ghostHPSlider.value = (float)ghostHP / GhostHPOrin;
            }
        }

        if (other.gameObject.CompareTag("BoomEX"))
        {
            ghostHP =0;
        }
    }
    void MoveToPlayer()
    {
        Transform player = _levelManager.characterDuck.transform;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= _levelManager.minDistanceToPlayer)
        {
            Vector3 directionToPlayer = transform.position - player.position;
            Vector3 targetPosition = transform.position + -directionToPlayer.normalized * _levelManager.minDistanceToPlayer;
            agent.SetDestination(targetPosition);
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
        GameObject effect = Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }



   
}
