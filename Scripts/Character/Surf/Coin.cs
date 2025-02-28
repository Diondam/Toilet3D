using System;
using System.Collections;
using System.Collections.Generic;
using Unicorn.Unicorn.Scripts.Controller.SurfGame;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isDead;
    public float destroyTime = 0.1f;

    void Update()
    {
        transform.position += Vector3.back * SurfLevelManager.Instance.enemySpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
           gameObject.SetActive(false);
        }
        
        if (collision.gameObject.CompareTag("Player") && SurfLevelManager.Instance.Result ==LevelResult.NotDecided)
        {
            isDead = true;
            SurfLevelManager.Instance.character.GetComponent<Animator>().SetTrigger("Attack");
            Invoke("DeActiveGameObject", destroyTime);
            int number = --SurfLevelManager.Instance.numbersNeedToWin;
            if (number == 0)
            {
                SurfLevelManager.Instance.Win();
            }
        }
    }

    void DeActiveGameObject()
    {
        var vfx = SurfLevelManager.Instance.vfx;
        vfx.transform.position = transform.GetChild(1).position;
        vfx.GetComponent<ParticleSystem>().Play();
        gameObject.SetActive(false);
    }

    public void isDeadTrue()
    {
        print("IS DEAD TRUE");
        isDead = true;
    }
}