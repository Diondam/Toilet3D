
using Unicorn.Unicorn.Scripts.Controller.CandyGame;
using UnityEngine;

public class CandyDam : MonoBehaviour
{
    public float destroyTime;
    public GameObject slashEffectPrefab;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && CandyLevelManager.Instance.Result == LevelResult.NotDecided)
        {
            // Assuming 'pooledObject' is the object you're about to reuse
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity; 
            gameObject.SetActive(false);
            GameObject effect = Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
            effect.transform.parent = CandyLevelManager.Instance.transform;
            int number = --CandyLevelManager.Instance.numbersNeedToWin;
            if (number == 0)
            {
                CandyLevelManager.Instance.Win();
            }
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            Invoke("DeActive", destroyTime);
        }
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
