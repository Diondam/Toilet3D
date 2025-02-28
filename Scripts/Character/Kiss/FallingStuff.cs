using Unicorn.Unicorn.Scripts.Controller.KissGame;
using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Character.Kiss
{
    public class FallingStuff : MonoBehaviour
    {
        KissLevelManager kmn => KissLevelManager.Instance;

        private void Start()
        {
            transform.parent = LevelManager.Instance.transform;
        }

        private bool isTap;
        private void Update()
        {
            //nếu vào vị trí có thể chém được
            if (transform.position.y <= kmn.heighWin && transform.position.y > kmn.heighWin - 0.2f &&
                kmn.Result == LevelResult.NotDecided)
            {
                kmn.canTap = true;
                kmn.readyToTap.gameObject.SetActive(true);
                print("Active image green");
            }
        }


        private bool isWin;

        private void OnCollisionEnter(Collision other)
        {
            if (other != null)
            {
                if (other.gameObject.CompareTag("Player") && !isTap && KissLevelManager.Instance.Result == LevelResult.NotDecided)
                {
                    isTap = true;
                    kmn.numberToWin--;
                    if (kmn.numberToWin == 0)
                    {
                        kmn.Win();
                    }
                }
                else if (other.gameObject.CompareTag("Finish") && !isTap && gameObject.transform.position.y<kmn.heighWin)
                {
                    kmn.End();
                }
            }
        }
    }
}