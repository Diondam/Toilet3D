using System;
using System.Collections;
using System.Collections.Generic;
using Unicorn.Unicorn.Scripts.Controller.DuckGame;
using UnityEngine;

namespace Unicorn
{
    public class TrapDuck : MonoBehaviour
    {
        private bool isAbleHP;

        void IsAble()
        {
            isAbleHP = true;
        }

        private void Update()
        {
            InvokeRepeating("IsAble", 0, 0.4f);
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("Player") && isAbleHP)
            {
                isAbleHP = false;
                DuckLevelManager.Instance.characterDuck.HPCharacter.value -= 0.1f;
                if (DuckLevelManager.Instance.characterDuck.HPCharacter.value == 0)
                {
                    DuckLevelManager.Instance.Lose();
                }
            }
        }
    }
}