using UnityEngine;

namespace Unicorn
{
    public class TrapSurf : MonoBehaviour
    {
        public float rangMoveSpeed = 20;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Finish"))
            {
                gameObject.SetActive(false);
            }
            if (other.gameObject.CompareTag("Player"))
            {
               other.gameObject.GetComponent<PlayerSurf>().HPCharacter.value -= 0.1f;
            }
        }

        void Update()
        {
            var RanNum = Random.Range(16, rangMoveSpeed);
            transform.position += Vector3.back * RanNum * Time.deltaTime;
        }
    }
}