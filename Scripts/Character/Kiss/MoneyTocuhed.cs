using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class MoneyTocuhed : MonoBehaviour
    {
        public int count=0;
        private GameObject boy;
        private GameObject girl;
        // Start is called before the first frame update
        void Start()
        {
            boy = GameObject.Find("Boy");
            girl = GameObject.Find("Girl");
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject == boy)
            {
                count++;
            }
            else if (other.gameObject == girl)
            {
                count++;
            }
            
            if(count ==2)
            {
                //print("Win");
                count = 0;
            }
        }

        
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
