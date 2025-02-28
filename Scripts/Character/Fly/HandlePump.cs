using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class HandlePump : MonoBehaviour
    {
        public Transform posThumb;
        void Start()
        {
            tempPre = transform.position.y;
        }
        float tempPre;
        float tempPos;
        public int count = 0;
        void Update()
        {
            FollowPump();
        }

        void FollowPump()
        {
            tempPos = posThumb.position.y;
            float chenh = (tempPos - tempPre);
            transform.position = new Vector3(transform.position.x, transform.position.y+ chenh,transform.position.z);
            tempPre = tempPos;
        }
    }
}
