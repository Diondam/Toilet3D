using Unicorn.Unicorn.Scripts.Controller.KissGame;
using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Character.Kiss
{
    public class BoomKiss : MonoBehaviour
    {
        public GameObject slashEffectPrefab;
        KissLevelManager kmn => KissLevelManager.Instance;

        private void Start()
        {
            transform.parent = LevelManager.Instance.transform;
        }
        private void Update()
        {
            if (transform.position.y <= kmn.heighWin && transform.position.y > kmn.heighWin-0.2f&& kmn.Result == LevelResult.NotDecided)
            {
                kmn.readyToTap.gameObject.SetActive(true);
                //kmn.HideImage();
            }
        }
       
        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                if (other.gameObject.CompareTag("Player") && KissLevelManager.Instance.Result == LevelResult.NotDecided)
                {
                    print("pumpkin đã va chạm player");
                    GameObject effect = Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
                    effect.transform.parent = kmn.transform;
                    Destroy(gameObject);
                    KissLevelManager.Instance.End();
                }
            }
            Invoke("Destroy", 1);
        }

        void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
