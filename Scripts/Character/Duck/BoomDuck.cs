using System;
using System.Collections;
using System.Collections.Generic;
using Unicorn.Unicorn.Scripts.Controller.DuckGame;
using UnityEngine;

namespace Unicorn
{
    public class BoomDuck : MonoBehaviour
    {
        
    public GameObject slashEffectPrefab;
    public GameObject BoomEX;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && DuckLevelManager.Instance.Result == LevelResult.NotDecided)
            {
                var boomEX = Instantiate(this.BoomEX, transform.position, Quaternion.identity);
                boomEX.transform.parent = transform;
                GameObject effect = Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
                Invoke("Destroy", 1);
            }
        }
        void Destroy()
        {
            Destroy(gameObject);
        }
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
