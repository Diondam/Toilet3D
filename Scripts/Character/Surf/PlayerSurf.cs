using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class PlayerSurf : MonoBehaviour
    {
        public Slider HPCharacter;
        // Start is called before the first frame update
        void Start()
        {
            if (GameManager.Instance.CurrentLevel >= 3)
            {
                HPCharacter.gameObject.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
