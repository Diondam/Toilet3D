using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class CharacterSurf : MonoBehaviour
    {
        // Start is called before the first frame update

        private Animator anim;
        void Start()
        {
            anim = GetComponent<Animator>();
            anim.SetTrigger("Idle");
            Invoke("ActiveRun", 0.5f);
        }

        void ActiveRun()
        {
            anim.SetBool("Run", true);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
