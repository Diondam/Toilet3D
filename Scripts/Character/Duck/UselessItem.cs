using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class UselessItem : MonoBehaviour
    {
        public int gold = 10;
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.Profile.AddGold(gold, "");
            }
        }
    }
}
