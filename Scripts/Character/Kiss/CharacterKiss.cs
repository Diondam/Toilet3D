using System;
using RayFire;
using UnityEngine;

namespace Unicorn
{
    public class CharacterKiss: MonoBehaviour
    {
        //public GameObject pumpkin;
        public Animator anim;
        private bool isPress;
        public int leftOrRight;


        private void Update()
        {
            print("leftORright: "+ leftOrRight);
            if (!isPress)
            {
                if (Input.GetMouseButtonDown(0)) // Kiểm tra xem có bao nhiêu ngón tay đang chạm vào màn hình
                {
                    isPress = true;
                    
                    anim.SetInteger("Catch",leftOrRight);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                anim.SetInteger("Catch",0);
                isPress = false;
                anim.SetBool("Idle",true);
            }
        }
    }
}